﻿using BasePoint.Core.Domain.Entities;
using BasePoint.Core.Domain.Entities.Interfaces;
using BasePoint.Core.Domain.Observer;
using BasePoint.Core.Domain.Repositories.Interfaces;
using BasePoint.Core.Extensions;
using BasePoint.Core.Shared;
using LinFu.DynamicProxy;
using System.Collections;

namespace BasePoint.Core.Domain.Interceptors
{
    public class EntityStateControlInterceptor : IEntityStateObserver, IEntityObserver, IInvokeWrapper
    {
        private readonly IBaseEntity _entity;
        private readonly List<Tuple<IBaseEntity, IBaseEntity>> _relatedSubParts;
        private readonly ProxyFactory _proxyFactory;
        private readonly EntityStateControlInterceptor _parentInterceptor;
        private bool _startStateControl;
        public void AfterInvoke(InvocationInfo info, object returnValue)
        {
        }

        public void BeforeInvoke(InvocationInfo info)
        {

        }

        public object DoInvoke(InvocationInfo info)
        {
            var startStateControl = _parentInterceptor?._startStateControl ?? _startStateControl;

            if (startStateControl && MethodIsAllowedToIntercept(info))
            {
                string propertyName = info.TargetMethod.Name.Substring(4);

                object newValue = info.Arguments[Constants.ZeroBasedFirstIndex];

                if (info.Target.PropertyIsUpdated(propertyName, newValue))
                {
                    _entity.SetStateAsUpdated();

                    UpdateParentState(_entity);
                }
            }

            return info.TargetMethod.Invoke(_entity, info.Arguments);
        }

        private void UpdateParentState(IBaseEntity entity)
        {
            var relatedSubParts = _parentInterceptor?._relatedSubParts ?? _relatedSubParts;

            var parents = relatedSubParts.Where(x => x.Item2 == entity);

            foreach (var parent in parents)
            {
                parent.Item1.SetStateAsUpdated();

                UpdateParentState(parent.Item1);
            }
        }

        private static bool MethodIsAllowedToIntercept(InvocationInfo info)
        {
            string methodName = info.TargetMethod.Name;

            if ((methodName != "NotifyPropertyUpdated") &&
                (methodName.Substring(0, 4) != "set_" ||
                 methodName == "set_State" ||
                 methodName == "set_PersistedValues" ||
                 methodName == "set_PropertiesUpdated"))
            {
                return false;
            }

            return true;
        }

        public EntityStateControlInterceptor(IBaseEntity entity, EntityStateControlInterceptor parentInterceptor) : this(entity)
        {
            _parentInterceptor = parentInterceptor;

            entity.AddObserver(this);
        }

        public EntityStateControlInterceptor(IBaseEntity entity)
        {
            _entity = entity;
            _proxyFactory = new ProxyFactory();
            _startStateControl = false;
            _relatedSubParts = [];
        }

        private void InitializeNestedProxies(Type realType, IBaseEntity realEntity, EntityStateControlInterceptor parentInterceptor)
        {
            var baseEntityProperties = realType.GetProperties()
                .Where(p => p.PropertyType.IsSubclassOf(typeof(BaseEntity)));

            var entityListProperties = realType.GetProperties()
               .Where(p => p.PropertyType.Name.Contains("IEntityList") || p.PropertyType.Name.Contains("EntityList"));

            foreach (var property in baseEntityProperties)
            {
                var propertyValue = (BaseEntity)property.GetValue(realEntity, null);

                if (propertyValue is not null)
                {
                    var propertyType = propertyValue.GetType();
                    var interceptor = new EntityStateControlInterceptor(propertyValue, parentInterceptor);
                    var proxy = _proxyFactory.CreateProxy(propertyType, interceptor);

                    property.SetValue(realEntity, (BaseEntity)proxy, null);

                    InitializeNestedProxies(propertyType, propertyValue, parentInterceptor);
                    parentInterceptor._relatedSubParts.Add(new Tuple<IBaseEntity, IBaseEntity>(realEntity, propertyValue));
                    interceptor._startStateControl = true;
                }
            }

            foreach (var property in entityListProperties)
            {
                var entityList = (property.GetValue(realEntity, null) as IList);

                if (entityList is not null)
                {
                    for (var i = 0; i < entityList.Count; i++)
                    {
                        var entityListItem = entityList[i];

                        if (entityListItem is not null)
                        {
                            var entityListItemType = entityListItem.GetType();
                            var interceptor = new EntityStateControlInterceptor((BaseEntity)entityListItem, parentInterceptor);

                            var proxy = _proxyFactory.CreateProxy(entityListItemType, interceptor);

                            entityList[i] = (BaseEntity)proxy;

                            InitializeNestedProxies(entityListItemType, (BaseEntity)entityListItem, parentInterceptor);
                            parentInterceptor._relatedSubParts.Add(new Tuple<IBaseEntity, IBaseEntity>(realEntity, (BaseEntity)entityListItem));
                            interceptor._startStateControl = true;
                        }
                    }
                }
            }
        }

        public T CreateEntityWihStateControl<T>(T entity) where T : IBaseEntity
        {
            var proxyEntity = _proxyFactory.CreateProxy(entity.GetType(), this);

            InitializeNestedProxies(entity.GetType(), entity, this);

            _startStateControl = true;

            return (T)proxyEntity;
        }

        public void NotifyEntityPropertyUpdate(IBaseEntity entity, string propertyName, object propertyValue)
        {
            entity.SetStateAsUpdated();

            UpdateParentState(entity);
        }
    }
}