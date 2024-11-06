using BasePoint.Core.Domain.Enumerators;
using BasePoint.Core.Shared;

namespace BasePoint.Core.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime FirstDayOfMonth(this DateTime inputDateTime)
        {
            var firstDay = new DateTime(inputDateTime.Year, inputDateTime.Month, Constants.QuantityOne);
            return firstDay;
        }

        public static DateTime LastDayOfMonth(this DateTime inputDateTime)
        {
            var nextMonth = new DateTime(inputDateTime.Year, inputDateTime.Month, Constants.QuantityOne).AddMonths(Constants.QuantityOne);
            return nextMonth.AddDays(Constants.QuantityMinusOne);
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
            return inputDateTime.AddDays(Constants.DaysInAWeek * numberOfWeeks);
        }

        public static int DaysSince(this DateTime date1, DateTime date2)
        {
            TimeSpan difference = date1 - date2;

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
            return (inputDateTime.DayOfWeek.IsWeekend());
        }

        public static DateTime NextDayAfter(this DateTime dateTime, DayOfWeek dayOfWeek, int weeksAhead = 1)
        {
            var daysToAdd = dateTime.DayOfWeek.DaysToNext(dayOfWeek);

            daysToAdd += weeksAhead * Constants.DaysInAWeek;

            return dateTime.AddDays(daysToAdd);
        }

        public static DateTime LastDayBefore(this DateTime dateTime, DayOfWeek dayOfWeek, int weeksbefore = 1)
        {
            var daysToSubtract = dateTime.DayOfWeek.DaysSinceLast(dayOfWeek);

            daysToSubtract += weeksbefore * Constants.DaysInAWeek;

            return dateTime.AddDays(-daysToSubtract);
        }

        public static List<DateTime> SplitIntoInstallments(
            this DateTime referenceDate,
            int numberOfInstallments,
            int intervalDays,
            WeekendInstallmentConfiguration weekendInstallmentConfiguration = WeekendInstallmentConfiguration.NoAction)
        {
            var installments = new List<DateTime>();

            var currentInstallment = Constants.FirstIndex;
            var currentDate = referenceDate;

            while (currentInstallment <= numberOfInstallments)
            {
                currentDate = currentDate.AddDays(intervalDays);

                if (currentDate.IsWeekend())
                {
                    if (weekendInstallmentConfiguration == WeekendInstallmentConfiguration.SetToLastFriday)
                    {
                        currentDate = currentDate.LastDayBefore(DayOfWeek.Friday, Constants.QuantityZero);
                    }
                    else if (weekendInstallmentConfiguration == WeekendInstallmentConfiguration.SetToNextMonday)
                    {
                        currentDate = currentDate.NextDayAfter(DayOfWeek.Monday, Constants.QuantityZero);
                    }
                }

                currentInstallment++;

                installments.Add(currentDate);
            }

            return installments;
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