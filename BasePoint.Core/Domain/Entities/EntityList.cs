using BasePoint.Core.Domain.Entities.Interfaces;
using BasePoint.Core.Domain.Enumerators;
using BasePoint.Core.Extensions;
using BasePoint.Core.Shared;
using System.Collections;

namespace BasePoint.Core.Domain.Entities
{
    public class EntityList<Entity> : IEntityList<Entity>, ICollection, IList
        where Entity : IBaseEntity
    {
        private readonly IList<Entity> _deletedItems;
        private readonly IList<Entity> _items;

        public IBaseEntity Parent { get; protected set; }

        public EntityList()
        {
            _deletedItems = [];
            _items = [];
            Parent = null;
        }

        public EntityList(IBaseEntity parent)
        {
            _deletedItems = [];
            _items = [];
            Parent = parent;
        }

        public EntityList(int capacity)
        {
            _deletedItems = new List<Entity>(capacity);
            _items = new List<Entity>(capacity);
            Parent = null;
        }

        public EntityList(IEntityList<Entity> entities)
        {
            _deletedItems = new List<Entity>(entities.DeletedItems);
            _items = new List<Entity>(entities.Items);
            Parent = null;
        }

        public EntityList(int capacity, IBaseEntity parent)
            : this(capacity)
        {
            Parent = parent;
        }

        public EntityList(IEntityList<Entity> entities, IBaseEntity parent)
            : this(entities)
        {
            Parent = parent;
        }

        public int IndexOf(Entity item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, Entity item)
        {
            _items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            if ((index < Constants.ZeroBasedFirstIndex) || (index >= Count))
                throw new ArgumentOutOfRangeException();

            Entity entity = _items.ElementAt(index);

            Remove(entity);
        }

        public void Add(Entity item)
        {
            if (item.State == EntityState.Deleted)
                _deletedItems.Add(item);
            else
                _items.Add(item);

            Parent?.SetStateAsUpdated();
        }

        public void AddRange(IEnumerable<Entity> items)
        {
            foreach (Entity item in items)
                Add(item);
        }

        public void Clear()
        {
            _items.Clear();
            _deletedItems.Clear();
        }

        public bool Contains(Entity item)
        {
            return _items.Contains(item);
        }

        public bool ContainsWithId(Guid entityId)
        {
            return _items.Any(item => item.Id == entityId);
        }

        public bool ContainsAnotherWhith<TKey>(Entity currentEntity, Func<Entity, TKey> propertySelector)
        {
            if (propertySelector == null)
                return false;

            TKey currentValue = propertySelector(currentEntity);
            return _items.Any(item => item.Id != currentEntity.Id && EqualityComparer<TKey>.Default.Equals(propertySelector(item), currentValue));
        }

        public bool HasMissingEntities(IEnumerable<Guid> entityIds)
        {
            return _items
                .SafeSelect(x => x.Id)
                .Any(item => !entityIds.Contains(item));
        }

        public bool HasMissingEntities(IEnumerable<Entity> items)
        {
            return HasMissingEntities(items.SafeSelect(x => x.Id));
        }

        public void CopyTo(Entity[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Entity item)
        {
            var itemIndex = _items.IndexOf(item);

            if (itemIndex < Constants.ZeroBasedFirstIndex)
                return false;

            _items.Remove(item);

            Parent?.SetStateAsUpdated();

            if (item.State is not EntityState.New)
            {
                item.SetStateAsDeleted();

                _deletedItems.Add(item);
            }

            return true;
        }

        public int RemoveAll(Predicate<Entity> match)
        {
            var itemsToRemove = _items.Where(i => match(i)).ToList();

            if ((itemsToRemove.Count() > Constants.QuantityZero) && (Parent != null))
            {
                Parent.SetStateAsUpdated();
            }

            foreach (var item in itemsToRemove)
            {
                var removedItem = _items.Remove(item);

                if (removedItem)
                {
                    if (item.State != EntityState.New)
                    {
                        item.SetStateAsDeleted();

                        _deletedItems.Add(item);
                    }
                }
            }

            return itemsToRemove.Count();
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return (IEnumerator<Entity>)((IEnumerable)this).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public IList<Entity> AllItems
        {
            get
            {
                List<Entity> allItems = [.. _deletedItems, .. _items];

                return allItems;
            }
        }

        public IList<Entity> Items
        {
            get { return _items; }
        }

        public IList<Entity> DeletedItems
        {
            get { return _deletedItems; }
        }

        public void CopyTo(Array array, int index)
        {
            _items.ToArray().CopyTo(array, index);
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        public int Add(object value)
        {
            this.Add((Entity)value);

            return _items.Count - 1;
        }

        public bool Contains(object value)
        {
            return _items.Contains((Entity)value);
        }

        public int IndexOf(object value)
        {
            return _items.IndexOf((Entity)value);
        }

        public void Insert(int index, object value)
        {
            _items.Insert(index, (Entity)value);
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            Remove((Entity)value);
        }

        object IList.this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                _items[index] = (Entity)value;
            }
        }

        public Entity this[int index]
        {
            get
            {
                object entity = ((IList)this)[index];

                return (Entity)entity;
            }
            set
            {
                ((IList)this)[index] = (Entity)value;
            }
        }

        public IEntityList<Entity> Clone()
        {
            var entities = new EntityList<Entity>();

            foreach (var item in _items)
            {
                var cloneEntity = (Entity)item.EntityClone();
                entities.Add(cloneEntity);
            }

            return entities;
        }
    }
}
