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
            var distinctItems = source.Select(item =>
                keySelectors.Select(selector => selector.Compile()(item)).ToList()).Distinct();

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
    }
}