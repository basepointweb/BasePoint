using BasePoint.Core.Domain.Cqrs;

namespace BasePoint.Core.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void AddComand(IEntityCommand command);

        Task<UnitOfWorkResult> SaveChangesAsync();

        Task RollbackAsync();

        void SetEntitiesPersistedState();
    }
}