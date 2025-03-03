using BasePoint.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace BasePoint.Core.Tests.Extensions
{
    public class IntegerExtensionTests
    {
        [Theory]
        [InlineData(5, 3, "2,2,1")]
        [InlineData(12, 4, "3,3,3,3")]
        [InlineData(12, 5, "3,3,2,2,2")]
        public void SplitIntoBalancedParts_PartSizeIsGreaterThanStringLength_ThrowsException(int inputInteger, int partsNumber, string expectedResultStr)
        {
            var expectedResultList = expectedResultStr.Split(',');

            var expectedResult = new int[expectedResultList.Length];

            var index = 0;

            expectedResultList.ForEach(x =>
            {
                expectedResult[index] = int.Parse(expectedResultList[index]);
                index++;
            });

            var parts = inputInteger.SplitIntoBalancedParts(partsNumber);
            index = 0;

            foreach (var part in parts)
            {
                part.Should().Be(expectedResult[index]);
                index++;
            }

            parts.Sum().Should().Be(inputInteger);

            parts.Should().HaveCount(expectedResultList.Length);
        }

        [Theory]
        [InlineData(3, 5)]
        [InlineData(1, 2)]
        [InlineData(0, 1)]
        public void SplitIntoBalancedParts_PartsNumberIsGreaterThanNumber_ThrowsException(int inputInteger, int partSize)
        {
            Action action = () => inputInteger.SplitIntoBalancedParts(partSize);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}