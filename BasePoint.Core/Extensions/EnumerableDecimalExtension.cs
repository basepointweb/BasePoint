using BasePoint.Core.Application.Dtos;

namespace BasePoint.Core.Extensions
{
    public static class EnumerableDecimalExtension
    {
        public static IEnumerable<AccumulatedDecimalValueItem> AccumulatedValues(this IEnumerable<decimal> decimalValues)
        {
            ArgumentNullException.ThrowIfNull(decimalValues);

            var currentAccumulatedValue = decimal.Zero;

            var accumulatedValues = decimalValues
                .Select((value, index) => new AccumulatedDecimalValueItem
                {
                    Index = index,
                    Value = currentAccumulatedValue += value
                })
                .ToList();

            return accumulatedValues;
        }

        public static decimal AccumulatedValueIn(this IEnumerable<decimal> decimalValues, int indexToCheck)
        {
            var accumulatedValues = AccumulatedValues(decimalValues);

            if (indexToCheck > accumulatedValues.LastIndex())
                throw new IndexOutOfRangeException();

            return accumulatedValues.ElementAt(indexToCheck).Value;
        }
    }
}