using BasePoint.Core.Domain.Entities.Interfaces;

namespace BasePoint.Core.Domain.Repositories.Interfaces
{
    public interface IEntityStateObserver
    {
        T CreateEntityWihStateControl<T>(T entity) where T : IBaseEntity;
    }
}