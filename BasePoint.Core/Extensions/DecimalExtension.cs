using BasePoint.Core.Shared;

namespace BasePoint.Core.Extensions
{
    public static class DecimalExtension
    {
        public static decimal NegativeIfPositive(this decimal value)
        {
            return NegateIf(value, value > decimal.Zero);
        }

        public static decimal NegateIf(this decimal value, bool negateCondition)
        {
            var result = value;

            if (negateCondition)
                result = Negate(result);

            return result;
        }

        public static decimal Negate(this decimal value)
        {
            return -value;
        }

        public static decimal DivideBy(this decimal? value1, decimal? value2, int? decimalPlaces = null)
        {
            if (!value1.HasValue || !value2.HasValue)
                return decimal.Zero;

            return value1.DivideBy(value2.Value, decimalPlaces);
        }

        public static decimal DivideBy(this decimal value1, decimal value2, int? decimalPlaces = null)
        {
            if ((value1 == decimal.Zero) || (value2 == decimal.Zero))
                return decimal.Zero;

            decimal result = value1 / value2;

            if (decimalPlaces.HasValue)
                result = Math.Round(result, decimalPlaces.Value);

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

        public static decimal PercentageOf(this decimal value1, decimal value2)
        {
            return value1 / value2;
        }

        public static decimal PercentageHundredBasedOf(this decimal value1, decimal value2)
        {
            return value1 / value2 * Constants.HandredBasedAHundredPercent;
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

        public static bool RoundedEquals(this decimal value1, decimal value2, int decimalPlaces = 2)
        {
            return (Math.Round(value1, decimalPlaces) == Math.Round(value2, decimalPlaces));
        }

        public static decimal Round(this decimal value1, int decimalPlaces = 2)
        {
            return Math.Round(value1, decimalPlaces);
        }

        public static List<decimal> SplitIntoInstallments(this decimal amount, int numberOfInstallments, bool resisualInFirstInstallment = true)
        {
            decimal installmentValue = Math.Floor(amount / numberOfInstallments * Constants.HandredBasedAHundredPercent) / Constants.HandredBasedAHundredPercent;

            decimal residualValue = amount - (installmentValue * numberOfInstallments);

            var installments = new List<decimal>();

            if (resisualInFirstInstallment)
            {
                installments.Add(installmentValue + residualValue);

                for (int i = Constants.ZeroBasedSecondIndex; i < numberOfInstallments; i++)
                {
                    installments.Add(installmentValue);
                }
            }
            else
            {
                for (int i = Constants.ZeroBasedFirstIndex; i < numberOfInstallments.ToZeroBasedIndex(); i++)
                {
                    installments.Add(installmentValue);
                }

                installments.Add(installmentValue + residualValue);
            }

            return installments;
        }

        public static decimal CompoundInterestMonthly(this decimal principal, decimal monthlyRate, int months)
        {
            double compoundFactor = Math.Pow((double)(Constants.AHundredPercent + monthlyRate), months);
            return principal * (decimal)compoundFactor;
        }

        public static decimal CompoundInterestYearly(this decimal principal, decimal annualRate, int years, int timesCompounded = 12)
        {
            double compoundFactor = Math.Pow((double)(Constants.AHundredPercent + annualRate) / timesCompounded, timesCompounded * years);
            return principal * (decimal)compoundFactor;
        }
    }
}
