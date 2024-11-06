using BasePoint.Core.Shared;

namespace BasePoint.Core.Extensions
{
    public static class DayOfWeekExtension
    {
        public static string BrazilianNameOfWeek(this DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Sunday => "Domingo",
                DayOfWeek.Monday => "Segunda-feira",
                DayOfWeek.Tuesday => "Terça-feira",
                DayOfWeek.Wednesday => "Quarta-feira",
                DayOfWeek.Thursday => "Quinta-feira",
                DayOfWeek.Friday => "Sexta-feira",
                DayOfWeek.Saturday => "Sábado",
                _ => "Domingo",
            };
        }

        public static DayOfWeek NextDay(this DayOfWeek dayOfWeek)
        {
            return (DayOfWeek)dayOfWeek.CyclingNextValue<DayOfWeek>();
        }

        public static DayOfWeek LastDay(this DayOfWeek dayOfWeek)
        {
            return (DayOfWeek)dayOfWeek.CyclingLastValue<DayOfWeek>();
        }

        public static int DaysToNext(this DayOfWeek dayOfWeek, DayOfWeek nextDay)
        {
            if (dayOfWeek == nextDay)
                return Constants.QuantityZero;

            var currentDay = dayOfWeek;
            int days = Constants.QuantityZero;

            while (currentDay != nextDay)
            {
                currentDay = currentDay.NextDay();
                days++;
            }

            return days;
        }

        public static int DaysSinceLast(this DayOfWeek dayOfWeek, DayOfWeek lastDay)
        {
            if (dayOfWeek == lastDay)
                return Constants.QuantityZero;

            var currentDay = dayOfWeek;
            int days = Constants.QuantityZero;

            while (currentDay != lastDay)
            {
                currentDay = currentDay.LastDay();
                days++;
            }

            return days;
        }

        public static bool IsWeekend(this DayOfWeek dayOfWeek)
        {
            return (dayOfWeek.In(DayOfWeek.Sunday, DayOfWeek.Saturday));
        }
    }
}
