using BasePoint.Core.Domain.Entities.Interfaces;
using BasePoint.Core.UnitOfWork.Interfaces;

namespace BasePoint.Core.Domain.Repositories.Interfaces
{
    public interface IRepository<Entity> where Entity : IBaseEntity
    {
        void Persist(Entity entity, IUnitOfWork unitOfWork);
        Task<Entity> GetById(Guid id);

        Task<IEnumerable<(Guid Id, Entity Entity)>> FetchByIds(IEnumerable<Guid> ids);
    }
}
