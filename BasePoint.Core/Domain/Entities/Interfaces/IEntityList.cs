namespace BasePoint.Core.Domain.Entities.Interfaces
{
    public interface IEntityList<Entity> : IList<Entity>
    {
        IBaseEntity Parent { get; }

        IList<Entity> AllItems { get; }

        IList<Entity> Items { get; }

        IList<Entity> DeletedItems { get; }

        void AddRange(IEnumerable<Entity> items);

        IEntityList<Entity> Clone();

        IEntityList<Entity> CloneWhere<TKey>(Predicate<Entity> match);

        int RemoveAll(Predicate<Entity> match);

        bool ContainsWithId(Guid entityId);

        bool ContainsAnotherWhith<TKey>(Entity currentEntity, Func<Entity, TKey> propertySelector);

        bool HasMissingEntities(IEnumerable<Guid> entityIds);

        bool HasMissingEntities(IEnumerable<Entity> items);

        IEnumerable<Entity> RemoveEntities(IEnumerable<Guid> entityIds);

        IEnumerable<Entity> RemoveMissingEntities(IEnumerable<Guid> entityIds);

        IEnumerable<Entity> RemoveWhereIn<TKey>(Func<Entity, TKey> propertySelector, IEnumerable<TKey> values);

        IEnumerable<Entity> RemoveWhereNotIn<TKey>(Func<Entity, TKey> propertySelector, IEnumerable<TKey> values);

        void Equalize(IEnumerable<Entity> anotherEntityList);
    }
}