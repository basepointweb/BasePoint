using BasePoint.Core.Domain.Entities.Interfaces;
using BasePoint.Core.Exceptions;
using BasePoint.Core.Shared;
using System.Linq.Expressions;

namespace BasePoint.Core.Extensions
{
    public static class EntityExtension
    {
        public static Entity ThrowResourceNotFoundIfIsNull<Entity>(this Entity entity, string errorMessage) where Entity : IBaseEntity
        {
            ResourceNotFoundException.ThrowIf(entity is null, errorMessage);

            return entity;
        }

        public static Entity ThrowInvalidInputIfIsNotNull<Entity>(this Entity entity, string errorMessage) where Entity : IBaseEntity
        {
            InvalidInputException.ThrowIf(entity is not null, errorMessage);

            return entity;
        }

        public static IEnumerable<Entity> ThrowInvalidInputIfIsHasItems<Entity>(this IEnumerable<Entity> entities, string errorMessage) where Entity : IBaseEntity
        {
            ResourceNotFoundException.ThrowIf(!entities.IsNullOrEmpty(), errorMessage);

            return entities;
        }

        public static Entity ThrowInvalidInputIfMatches<Entity>(this Entity entity, Predicate<Entity> match, string errorMessage) where Entity : IBaseEntity
        {
            InvalidInputException.ThrowIf(match(entity), errorMessage);

            return entity;
        }

        public static IEnumerable<Entity> ThrowInvalidInputIfAtLeastAnItemMatches<Entity>(this IEnumerable<Entity> entities, Predicate<Entity> match, string errorMessage) where Entity : IBaseEntity
        {
            var matchedItems = entities.Where(i => match(i)).ToList();

            InvalidInputException.ThrowIf(matchedItems.Count != Constants.QuantityZero, errorMessage);

            return entities;
        }

        public static IEnumerable<Entity> ThrowInvalidInputIfAllItemsMatches<Entity>(this IEnumerable<Entity> entities, Predicate<Entity> match, string errorMessage) where Entity : IBaseEntity
        {
            var matchedItems = entities.Where(i => match(i)).ToList();

            InvalidInputException.ThrowIf(matchedItems.Count == entities.Count(), errorMessage);

            return entities;
        }

        public static IEnumerable<Entity> ThrowInvalidInputIfDoesNotMatch<Entity>(this IEnumerable<Entity> entities, Predicate<Entity> match, string errorMessage) where Entity : IBaseEntity
        {
            InvalidInputException.ThrowIf(!entities.Any(i => match(i)), errorMessage);

            return entities;
        }

        public static IEnumerable<Entity> ThrowInvalidInputIfHasDuplicates<Entity, T, TKey>(this IEnumerable<Entity> entities, string errorMessage, params Expression<Func<Entity, TKey>>[] keySelectors) where Entity : IBaseEntity
        {
            InvalidInputException.ThrowIf(entities.HasDuplicates(keySelectors), errorMessage);

            return entities;
        }

        public static Entity ThrowInvalidInputIfDoesNotMatch<Entity>(this Entity entity, Predicate<Entity> match, string errorMessage) where Entity : IBaseEntity
        {
            InvalidInputException.ThrowIf(match(entity), errorMessage);

            return entity;
        }
    }
}