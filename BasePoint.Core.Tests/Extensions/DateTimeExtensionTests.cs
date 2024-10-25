using BasePoint.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace BasePoint.Core.Tests.Extensions
{
    public class DateTimeExtensionTests
    {
        [Theory]
        [InlineData(DayOfWeek.Tuesday, 0, "2024-10-22")]
        [InlineData(DayOfWeek.Wednesday, 0, "2024-10-23")]
        [InlineData(DayOfWeek.Tuesday, 1, "2024-10-29")]
        [InlineData(DayOfWeek.Wednesday, 1, "2024-10-30")]
        [InlineData(DayOfWeek.Monday, 0, "2024-10-28")]
        [InlineData(DayOfWeek.Monday, 1, "2024-11-04")]
        public void NextDayAfter_Always_ReturnsNextCorrespondentDate(
            DayOfWeek dayOfWeek,
            int weeksAhead,
            string expectedDateStr)
        {
            var currentDate = new DateTime(2024, 10, 22);

            var expectedDate = DateTime.Parse(expectedDateStr);

            var resultDate = currentDate.NextDayAfter(dayOfWeek, weeksAhead);

            resultDate.Should().Be(expectedDate);
        }
    }
}
