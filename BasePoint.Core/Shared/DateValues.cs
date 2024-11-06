using BasePoint.Core.Extensions;

namespace BasePoint.Core.Shared
{
    public static class DateValues
    {
        public static readonly DateTime Tomorrow = DateTime.Today.AddDays(Constants.QuantityOne);
        public static readonly DateTime UtcTomorrow = DateTime.UtcNow.AddDays(Constants.QuantityOne);
        public static readonly DateTime Yesterday = DateTime.Today.AddDays(Constants.QuantityMinusOne);
        public static readonly DateTime UtcYesterday = DateTime.UtcNow.AddDays(Constants.QuantityMinusOne);
        public static readonly DateTime FirstDayOfThisMonth = DateTime.Today.FirstDayOfMonth();
        public static readonly DateTime LastDayOfThisMonth = DateTime.Today.LastDayOfMonth();
    }
}
