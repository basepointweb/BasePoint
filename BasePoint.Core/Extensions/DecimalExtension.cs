using BasePoint.Core.Shared;

namespace BasePoint.Core.Extensions
{
    public static class DecimalExtension
    {
        public static decimal NegativeIfPositive(this decimal value)
        {
            var result = value;

            if (value > decimal.Zero)
                result = -value;

            return result;
        }

        public static decimal NegateIf(this decimal value, bool negateCondition)
        {
            var result = value;

            if (negateCondition)
                result = -value;

            return result;
        }

        public static decimal Negate(this decimal value)
        {
            return -value;
        }

        public static decimal DivedeBy(this decimal? value1, decimal? value2, int? decimalPlaces = null)
        {
            if (!value1.HasValue || !value2.HasValue)
                return decimal.Zero;

            return value1.DivedeBy(value2.Value, decimalPlaces);
        }

        public static decimal DivedeBy(this decimal value1, decimal value2, int? decimalPlaces = null)
        {
            if (value1 == decimal.Zero)
                return decimal.Zero;

            decimal result = value1 / value2;

            if (decimalPlaces.HasValue)
            {
                result = Math.Round(result, decimalPlaces.Value);
            }

            return result;
        }

        public static decimal RemainingHundredBasedPercentage(this decimal percentage)
        {
            return Constants.HandredBasedAHundredPercent - percentage;
        }

        public static decimal RemainingPercentage(this decimal percentage)
        {
            return Constants.AHundredPercent - percentage;
        }

        public static decimal PercentualValue(this decimal value, decimal percentage)
        {
            return value * percentage;
        }

        public static decimal PercentualHandredBasedValue(this decimal value, decimal percentage)
        {
            return value * percentage / Constants.HandredBasedAHundredPercent;
        }

        public static decimal IncreasedPercentualValue(this decimal value, decimal percentage)
        {
            return value * (Constants.AHundredPercent + percentage);
        }

        public static decimal IncreasedHandredBasedPercentualValue(this decimal value, decimal percentage)
        {
            return value * (Constants.AHundredPercent + (percentage / Constants.HandredBasedAHundredPercent));
        }

        public static bool IsBetween(this decimal value, decimal initialValue, decimal finalValue)
        {
            return (value > initialValue) && (value < finalValue);
        }

        public static bool IsBetweenInclusive(this decimal value, decimal initialValue, decimal finalValue)
        {
            return (value >= initialValue) && (value <= finalValue);
        }

        public static bool RoundedEqualsTo(this decimal value1, decimal value2, int decimalPlaces = 2)
        {
            return (Math.Round(value1, decimalPlaces) == Math.Round(value2, decimalPlaces));
        }
    }
}
