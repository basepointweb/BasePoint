using BasePoint.Core.Domain.Entities.Interfaces;

namespace BasePoint.Core.Domain.Cqrs
{
    public interface IEntityCommand
    {
        IBaseEntity AffectedEntity { get; }
        Task<bool> ExecuteAsync();
    }
}