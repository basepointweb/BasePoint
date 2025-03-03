using BasePoint.Core.Application.Dtos;

namespace BasePoint.Core.Extensions
{
    public static class EnumerableStringExtension
    {
        public static IEnumerable<AccumulatedStringValueItem> AccumulatedValues(this IEnumerable<string> stringValues)
        {
            ArgumentNullException.ThrowIfNull(stringValues);

            var currentAccumulatedValue = string.Empty;

            var accumulatedValues = stringValues
                .Select((value, index) => new AccumulatedStringValueItem
                {
                    Index = index,
                    Value = currentAccumulatedValue += value
                })
                .ToList();

            return accumulatedValues;
        }

        public static string AccumulatedValueIn(this IEnumerable<string> stringValues, int indexToCheck)
        {
            var accumulatedValues = AccumulatedValues(stringValues);

            if (indexToCheck > accumulatedValues.LastIndex())
                throw new IndexOutOfRangeException();

            return accumulatedValues.ElementAt(indexToCheck).Value;
        }

        public static string ShortestString(this IEnumerable<string> stringValues)
        {
            return stringValues.SafeSelect(x => (x, x.Length))
                .OrderByDescending(x => x.Length)
                .First().x;
        }

        public static string LongestString(this IEnumerable<string> stringValues)
        {
            return stringValues.SafeSelect(x => (x, x.Length))
                .OrderBy(x => x.Length)
                .First().x;
        }
    }
}