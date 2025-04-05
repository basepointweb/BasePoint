using BasePoint.Core.Shared;
using System.Linq.Expressions;

namespace BasePoint.Core.Extensions
{
    public record GroupingResult<T>
    {
        public int DistinctCount { get; init; }
        public int TotalItems { get; init; }
        public T MaxItem { get; init; }
        public T MinItem { get; init; }
    }

    public static class EnumerableExtension
    {
        public static GroupingResult<T> Grouping<T>(
            this IEnumerable<T> source,
            params Expression<Func<T, object>>[] keySelectors)
        {
            var distinctItems = source.SafeSelect(item =>
                keySelectors.SafeSelect(selector => selector.Compile()(item)).ToList()).Distinct();

            return new GroupingResult<T>
            {
                DistinctCount = distinctItems.Count(),
                TotalItems = source.Count(),
                MaxItem = source.Max(),
                MinItem = source.Min()
            };
        }

        public static Dictionary<T, int> FrequencyDistribution<T>(
            this IEnumerable<T> source)
        {
            return source
             .GroupBy(item => item)
             .ToDictionary(g => g.Key, g => g.Count());
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        public static bool AnyNull<T>(this IEnumerable<T> collection)
        {
            return collection == null || collection.Any(x => x is null);
        }

        public static bool AllNull<T>(this IEnumerable<T> collection)
        {
            return collection == null || collection.Count(x => x is null) == collection.Count();
        }

        public static bool HasDuplicates<T, TKey>(this IEnumerable<T> source, params Expression<Func<T, TKey>>[] keySelectors)
        {
            var uniqueKeys = new HashSet<string>();

            foreach (var item in source)
            {
                var keyValues = keySelectors.Select(selector =>
                {
                    var compiledSelector = selector.Compile();
                    return compiledSelector(item);
                });

                var compositeKey = string.Join("|", keyValues);

                if (!uniqueKeys.Add(compositeKey))
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<TResult> SafeSelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source?.Select(selector) ?? Enumerable.Empty<TResult>();
        }

        public static int LastIndex<T>(this IEnumerable<T> source)
        {
            return source.IsNullOrEmpty() ? Constants.QuantityMinusOne : source.Count().ToZeroBasedIndex();
        }

        public static IEnumerable<T> WhereIn<T, TKey>(this IEnumerable<T> source, Func<T, TKey> propertySelector, params TKey[] values)
        {
            return WhereIn(source, propertySelector, values.AsEnumerable());
        }

        public static IEnumerable<T> WhereIn<T, TKey>(this IEnumerable<T> source, Func<T, TKey> propertySelector, IEnumerable<TKey> values)
        {

            if (source == null || propertySelector == null || values == null)
                return Enumerable.Empty<T>();

            HashSet<TKey> valueSet = new HashSet<TKey>(values);
            return source.Where(item => valueSet.Contains(propertySelector(item)));
        }

        public static IEnumerable<T> WhereNotIn<T, TKey>(this IEnumerable<T> source, Func<T, TKey> propertySelector, params TKey[] values)
        {
            return WhereNotIn(source, propertySelector, values.AsEnumerable());
        }

        public static IEnumerable<T> WhereNotIn<T, TKey>(this IEnumerable<T> source, Func<T, TKey> propertySelector, IEnumerable<TKey> values)
        {

            if (source == null || propertySelector == null || values == null)
                return Enumerable.Empty<T>();

            HashSet<TKey> valueSet = new HashSet<TKey>(values);
            return source.Where(item => !valueSet.Contains(propertySelector(item)));
        }

        public static void RoundProperty<T>(this IEnumerable<T> source, Expression<Func<T, decimal>> propertyExpression, int decimals = 2)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            ArgumentNullException.ThrowIfNull(propertyExpression, nameof(propertyExpression));

            if (!(propertyExpression.Body is MemberExpression memberExpression))
                throw new ArgumentException("The expression must reference a property.");

            var propertyInfo = memberExpression.Member as System.Reflection.PropertyInfo;
            if (propertyInfo == null || !propertyInfo.CanWrite)
                throw new InvalidOperationException("The property must be of type decimal and have a public setter.");

            foreach (var item in source)
            {
                var originalValue = (decimal)propertyInfo.GetValue(item);
                var roundedValue = Math.Round(originalValue, decimals);
                propertyInfo.SetValue(item, roundedValue);
            }
        }

        public static IEnumerable<T> AllMinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
            where TKey : IComparable<TKey>
        {
            if (source.IsNullOrEmpty())
                return Enumerable.Empty<T>();

            var minValue = source.Min(selector);

            return source.Where(item => selector(item).CompareTo(minValue) == Constants.ComparisonEquals);
        }

        public static IEnumerable<T> AllMaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
            where TKey : IComparable<TKey>
        {
            if (source.IsNullOrEmpty())
                return Enumerable.Empty<T>();

            var maxValue = source.Max(selector);

            return source.Where(item => selector(item).CompareTo(maxValue) == Constants.ComparisonEquals);
        }
    }
}