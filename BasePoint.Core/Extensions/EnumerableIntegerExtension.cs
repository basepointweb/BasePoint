using BasePoint.Core.Application.Dtos;
using BasePoint.Core.Shared;

namespace BasePoint.Core.Extensions
{
    public static class EnumerableIntegerExtension
    {
        public static IEnumerable<int> EvenValues(this IEnumerable<int> integerValues)
        {
            return integerValues.Where(x => x.IsEven());
        }

        public static IEnumerable<int> OddValues(this IEnumerable<int> integerValues)
        {
            return integerValues.Where(x => !x.IsEven());
        }

        public static IEnumerable<AccumulatedIntegerValueItem> AccumulatedValues(this IEnumerable<int> integerValues)
        {
            ArgumentNullException.ThrowIfNull(integerValues);

            var currentAccumulatedValue = Constants.Zero;

            var accumulatedValues = integerValues
                .Select((value, index) => new AccumulatedIntegerValueItem
                {
                    Index = index,
                    Value = currentAccumulatedValue += value
                })
                .ToList();

            return accumulatedValues;
        }

        public static int AccumulatedValueIn(this IEnumerable<int> integerValues, int indexToCheck)
        {
            var accumulatedValues = AccumulatedValues(integerValues);

            if (indexToCheck > accumulatedValues.LastIndex())
                throw new IndexOutOfRangeException();

            return accumulatedValues.ElementAt(indexToCheck).Value;
        }
    }
}