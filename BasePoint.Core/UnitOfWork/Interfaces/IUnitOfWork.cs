using BasePoint.Core.Domain.Cqrs;

namespace BasePoint.Core.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void AddComand(IEntityCommand command);

        Task<bool> SaveChangesAsync();

        Task RollbackAsync();

        void SetEntitiesPersistedState();
    }
}