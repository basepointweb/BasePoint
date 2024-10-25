namespace BasePoint.Core.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime Tomorrow()
        {
            return DateTime.Now.AddDays(1);
        }

        public static DateTime UtcTomorrow()
        {
            return DateTime.UtcNow.AddDays(1);
        }

        public static DateTime Yesterday()
        {
            return DateTime.Now.AddDays(-1);
        }

        public static DateTime UtcYesterday()
        {
            return DateTime.UtcNow.AddDays(-1);
        }

        public static DateTime FirstDayOfMonth(this DateTime inputDateTime)
        {
            var firstDay = new DateTime(inputDateTime.Year, inputDateTime.Month, 1);
            return firstDay;
        }

        public static DateTime LastDayOfMonth(this DateTime inputDateTime)
        {
            var nextMonth = new DateTime(inputDateTime.Year, inputDateTime.Month, 1).AddMonths(1);
            return nextMonth.AddDays(-1);
        }

        public static DateTime LastSecond(this DateTime inputDateTime)
        {
            return new DateTime(inputDateTime.Year, inputDateTime.Month, inputDateTime.Day, 23, 59, 59);
        }

        public static DateTime LastSecondOfMonth(this DateTime inputDateTime)
        {
            var lastDay = inputDateTime.LastDayOfMonth();

            return new DateTime(lastDay.Year, lastDay.Month, lastDay.Day, 23, 59, 59);
        }

        public static DateTime AddWeeks(this DateTime inputDateTime, int numberOfWeeks)
        {
            return inputDateTime.AddDays(7 * numberOfWeeks);
        }

        public static int DaysBetween(this DateTime date1, DateTime date2)
        {
            var firstDate = date1 < date2 ? date1 : date2;
            var secondDate = date2 > date1 ? date2 : date1;

            TimeSpan difference = firstDate - secondDate;

            return difference.Days;
        }

        public static bool IsBetweenInclusive(this DateTime inputDateTime, DateTime inputInitialDateTime, DateTime inputFinalDateTime)
        {
            return ((inputDateTime >= inputInitialDateTime) && (inputDateTime <= inputFinalDateTime));
        }

        public static bool IsBetween(this DateTime inputDateTime, DateTime inputInitialDateTime, DateTime inputFinalDateTime)
        {
            return ((inputDateTime > inputInitialDateTime) && (inputDateTime < inputFinalDateTime));
        }

        public static bool IsWeekend(this DateTime inputDateTime)
        {
            return (inputDateTime.DayOfWeek.In(DayOfWeek.Sunday, DayOfWeek.Saturday));
        }

        public static DateTime NextDayAfter(this DateTime dateTime, DayOfWeek dayOfWeek, int weeksAhead = 1)
        {
            var daysToAdd = dateTime.DayOfWeek.DaysToNext(dayOfWeek);

            daysToAdd += weeksAhead * 7;

            return dateTime.AddDays(daysToAdd);
        }

        public static string ToBrazilianFormatWithHour(this DateTime inputDateTime)
        {
            return inputDateTime.ToString("dd/MM/yyyy hh:mm:ss");
        }

        public static string ToBrazilianFormat(this DateTime inputDateTime)
        {
            return inputDateTime.ToString("dd/MM/yyyy");
        }

        public static string ToAmericanFormatWithHour(this DateTime inputDateTime)
        {
            return inputDateTime.ToString("yyyy/MM/dd hh:mm:ss");
        }

        public static string ToAmericanFormat(this DateTime inputDateTime)
        {
            return inputDateTime.ToString("yyyy/MM/dd");
        }

        public static string BrazilianMonthName(this DateTime date)
        {
            var monthName = string.Empty;
            switch (date.Month)
            {
                case 1:
                    monthName = "Janeiro";
                    break;
                case 2:
                    monthName = "Fevereiro";
                    break;
                case 3:
                    monthName = "Março";
                    break;
                case 4:
                    monthName = "Abril";
                    break;
                case 5:
                    monthName = "Maio";
                    break;
                case 6:
                    monthName = "Junho";
                    break;
                case 7:
                    monthName = "Julho";
                    break;
                case 8:
                    monthName = "Agosto";
                    break;
                case 9:
                    monthName = "Setembro";
                    break;
                case 10:
                    monthName = "Outubro";
                    break;
                case 11:
                    monthName = "Novembro";
                    break;
                case 12:
                    monthName = "Dezembro";
                    break;
                default:
                    break;
            }

            return monthName;
        }
    }
}