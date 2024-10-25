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

        public static int DaysToNext(this DayOfWeek dayOfWeek, DayOfWeek nextDay)
        {
            if (dayOfWeek == nextDay)
                return 0;

            var currentDay = dayOfWeek;
            int days = 0;

            while (currentDay != nextDay)
            {
                currentDay = currentDay.NextDay();
                days++;
            }

            return days;
        }
    }
}
