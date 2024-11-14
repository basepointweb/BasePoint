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
    }
}
