using BasePoint.Core.Shared;

namespace BasePoint.Core.Extensions
{
    public static class IntegerExtension
    {
        public static int ToZeroBasedIndex(this int integerValue)
        {
            return integerValue - Constants.QuantityOne;
        }

        public static int FromZeroBasedIndex(this int integerValue)
        {
            return integerValue + Constants.QuantityOne;
        }

        public static bool IsEven(this int integerValue)
        {
            return (integerValue % 2 == Constants.Zero);
        }

        public static List<int> SplitIntoBalancedParts(this int number, int parts)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(number, parts, "The number of parts must be less than or equals to number");

            var result = new List<int>();

            int baseValue = number / parts;
            int remainder = number % parts;

            for (int i = 0; i < parts; i++)
            {
                if (i < remainder)
                    result.Add(baseValue + 1);
                else
                    result.Add(baseValue);
            }

            return result;
        }
    }
}
