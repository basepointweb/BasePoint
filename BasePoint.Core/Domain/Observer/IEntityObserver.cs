using BasePoint.Core.Domain.Entities.Interfaces;

namespace BasePoint.Core.Domain.Observer
{
    public interface IEntityObserver
    {
        void NotifyEntityPropertyUpdate(IBaseEntity entity, string propertyName, object propertyValue);
    }
}