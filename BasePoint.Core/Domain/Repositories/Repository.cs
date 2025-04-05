using BasePoint.Core.Domain.Cqrs;
using BasePoint.Core.Domain.Cqrs.CommandProviders;
using BasePoint.Core.Domain.Entities.Interfaces;
using BasePoint.Core.Domain.Enumerators;
using BasePoint.Core.Domain.Interceptors;
using BasePoint.Core.Domain.Repositories.Interfaces;
using BasePoint.Core.UnitOfWork.Interfaces;

namespace BasePoint.Core.Domain.Repositories
{
    public abstract class Repository<Entity> : IRepository<Entity> where Entity : IBaseEntity
    {
        private readonly ICqrsCommandProvider<Entity> _commandProvider;

        private readonly Dictionary<EntityState, Func<Entity, IEntityCommand>> _persistenceMethods = [];

        protected Repository(
            ICqrsCommandProvider<Entity> commandProvider)
        {
            _commandProvider = commandProvider;

            _persistenceMethods[EntityState.New] = _commandProvider.GetAddCommand;
            _persistenceMethods[EntityState.Updated] = _commandProvider.GetUpdateCommand;
            _persistenceMethods[EntityState.Deleted] = _commandProvider.GetDeleteCommand;
            _persistenceMethods[EntityState.PersistedDeleted] = _commandProvider.GetDeleteCommand;
        }

        public virtual void Persist(Entity entity, IUnitOfWork unitOfWork)
        {
            var persistenceMethod = _persistenceMethods.GetValueOrDefault(entity.State);

            if (persistenceMethod is not null)
                unitOfWork.AddComand(persistenceMethod(entity));
        }

        public virtual async Task<Entity> GetById(Guid id)
        {
            return HandleAfterGetFromCommandProvider(await _commandProvider.GetById(id));
        }

        public virtual async Task<IEnumerable<(Guid Id, Entity Entity)>> FetchByIds(IEnumerable<Guid> ids)
        {
            var results = await Task.WhenAll(ids.Select(async id => (id, await _commandProvider.GetById(id))));

            var fetchedEntities = new List<(Guid, Entity)>();

            foreach (var result in results)
            {
                var handledEntity = HandleAfterGetFromCommandProvider(result.Item2);

                fetchedEntities.Add((result.Item2.Id, handledEntity));
            }

            return fetchedEntities;
        }

        protected virtual T HandleAfterGetFromCommandProvider<T>(T entity) where T : IBaseEntity
        {
            if (entity is null)
                return default;

            entity.SetStateAsUnchanged();

            var interceptor = new EntityStateControlInterceptor(entity);

            return interceptor.CreateEntityWihStateControl(entity);
        }
    }
}