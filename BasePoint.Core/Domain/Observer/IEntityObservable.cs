namespace BasePoint.Core.Domain.Observer
{
    public interface IEntityObservable
    {
        void AddObserver(IEntityObserver observer);

        void RemoveObserver(IEntityObserver observer);

        void NotifyEntityObserversPropertyUpdate(string propertyName, object propertyValue);
    }
}