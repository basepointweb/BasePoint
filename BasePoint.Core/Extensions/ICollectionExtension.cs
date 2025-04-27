using BasePoint.Core.Exceptions;
using System.Linq.Expressions;

namespace BasePoint.Core.Extensions
{
    public static class ICollectionExtension
    {

        public static void AddIfNotExists<T, TKey1, TKey2>(
        this ICollection<T> source,
        T newItem,
        string errorMessageToThrowIfExists,
        params Expression<Func<T, object>>[] propertySelectors)
        {
        }

        public static void AddIfNotExists<T, TKey1, TKey2, TKey3>(
        this ICollection<T> source,
        T newItem,
        string errorMessageToThrowIfExists,
        params Expression<Func<T, object>>[] propertySelectors)
        {
        }

        public static void AddIfNotExists<T, TKey1>(
            this ICollection<T> source,
            T newItem,
            Expression<Func<T, TKey1>> propertySelector1,
            string errorMessageToThrowIfExists = null)
        {
            AddIfNotExistsInternal(source, newItem, errorMessageToThrowIfExists, propertySelector1);
        }

        public static void AddIfNotExists<T, TKey1, TKey2>(
            this ICollection<T> source,
            T newItem,
            Expression<Func<T, TKey1>> propertySelector1,
            Expression<Func<T, TKey2>> propertySelector2,
            string errorMessageToThrowIfExists = null)
        {
            AddIfNotExistsInternal(source, newItem, errorMessageToThrowIfExists, propertySelector1, propertySelector2);
        }

        public static void AddIfNotExists<T, TKey1, TKey2, TKey3>(
            this ICollection<T> source,
            T newItem,
            Expression<Func<T, TKey1>> propertySelector1,
            Expression<Func<T, TKey2>> propertySelector2,
            Expression<Func<T, TKey3>> propertySelector3,
            string errorMessageToThrowIfExists = null)
        {
            AddIfNotExistsInternal(source, newItem, errorMessageToThrowIfExists, propertySelector1, propertySelector2, propertySelector3);
        }

        public static void AddIfNotExists<T, TKey1, TKey2, TKey3, TKey4>(
            this ICollection<T> source,
            T newItem,
            Expression<Func<T, TKey1>> propertySelector1,
            Expression<Func<T, TKey2>> propertySelector2,
            Expression<Func<T, TKey3>> propertySelector3,
            Expression<Func<T, TKey4>> propertySelector4,
            string errorMessageToThrowIfExists)
        {
            AddIfNotExistsInternal(source, newItem, errorMessageToThrowIfExists, propertySelector1, propertySelector2, propertySelector3, propertySelector4);
        }

        public static void AddIfNotExists<T, TKey1, TKey2, TKey3, TKey4, TKey5>(
            this ICollection<T> source,
            T newItem,
            Expression<Func<T, TKey1>> propertySelector1,
            Expression<Func<T, TKey2>> propertySelector2,
            Expression<Func<T, TKey3>> propertySelector3,
            Expression<Func<T, TKey4>> propertySelector4,
            Expression<Func<T, TKey5>> propertySelector5,
            string errorMessageToThrowIfExists)
        {
            AddIfNotExistsInternal(source, newItem, errorMessageToThrowIfExists, propertySelector1, propertySelector2, propertySelector3, propertySelector4, propertySelector5);
        }

        private static void AddIfNotExistsInternal<T>(
            ICollection<T> source,
            T newItem,
            string errorMessageToThrowIfExists,
            params LambdaExpression[] propertySelectors)
        {
            var propertyFuncs = propertySelectors.Select(ps => (Func<T, object>)(item => ps.Compile().DynamicInvoke(item))).ToList();

            bool itemExists = source.Any(existingItem =>
                propertyFuncs.All(func =>
                {
                    var existingValue = func(existingItem);
                    var newValue = func(newItem);
                    return existingValue?.Equals(newValue) ?? newValue == null;
                }));

            if (!itemExists)
            {
                source.Add(newItem);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(errorMessageToThrowIfExists))
                    throw new ValidationException(errorMessageToThrowIfExists);
            }
        }
    }
}