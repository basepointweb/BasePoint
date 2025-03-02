using BasePoint.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace BasePoint.Core.Tests.Extensions
{
    public class StringExtensionTests
    {
        [Theory]
        [InlineData("#123|45#678|9", "123,678")]
        [InlineData("#123#45#678|9", "123#45#678")]
        [InlineData("123|45#678|9", "678")]
        [InlineData("123|45678|9", "")]
        [InlineData("#12345#6789", "")]
        public void SubstringsBetween_Should_ReturnListOfStrings(string inputString, string expectedPlainString)
        {
            var expectedStrings = expectedPlainString.IsEmpty() ? [] : expectedPlainString.Split(",");

            var resultStrings = inputString.SubstringsBetween("#", "|");

            foreach (var expectedString in expectedStrings)
            {
                resultStrings.Should().Contain(expectedString);
            }

            resultStrings.Should().HaveCount(expectedStrings.Length);
        }

        [Theory]
        [InlineData("#123|45#678|9", "123|45")]
        [InlineData("#123|45#678#9", "123|45,678")]
        public void SubstringsBetween_DelimitersAreSame_ReturnListOfStrings(string inputString, string expectedPlainString)
        {
            var expectedStrings = expectedPlainString.IsEmpty() ? [] : expectedPlainString.Split(",");

            var resultStrings = inputString.SubstringsBetween("#", "#");

            foreach (var expectedString in expectedStrings)
            {
                resultStrings.Should().Contain(expectedString);
            }

            resultStrings.Should().HaveCount(expectedStrings.Length);
        }

        [Theory]
        [InlineData("12345", 0, "")]
        [InlineData("", 1, "")]
        [InlineData("12345", 1, "1,2,3,4,5")]
        [InlineData("12345", 2, "12,34,5")]
        [InlineData("123456", 2, "12,34,56")]
        [InlineData("12345", 3, "123,45")]
        [InlineData("12345678", 3, "123,456,78")]
        [InlineData("12345", 4, "1234,5")]
        [InlineData("12345", 5, "12345")]
        public void SplitBySize_PartSizeIsLessThanOrEqualsStringLengthAndSmallerPartIntheEnd_ReturnListOfStrings(string inputString, int partsSize, string expectedPlainString)
        {
            var expectedStrings = expectedPlainString.IsEmpty() ? [] : expectedPlainString.Split(",");

            var resultStrings = inputString.SplitBySize(partsSize);

            foreach (var expectedString in expectedStrings)
            {
                resultStrings.Should().Contain(expectedString);
            }

            resultStrings.Should().HaveCount(expectedStrings.Length);
        }

        [Theory]
        [InlineData("12345", 0, "")]
        [InlineData("", 1, "")]
        [InlineData("12345", 1, "1,2,3,4,5")]
        [InlineData("12345", 2, "1,23,45")]
        [InlineData("123456", 2, "12,34,56")]
        [InlineData("12345", 3, "12,345")]
        [InlineData("12345678", 3, "12,345,678")]
        [InlineData("12345", 4, "1,2345")]
        [InlineData("12345", 5, "12345")]
        public void SplitBySize_PartSizeIsLessThanOrEqualsStringLengthAndSmallerPartInTheBegining_ReturnListOfStrings(string inputString, int partsSize, string expectedPlainString)
        {
            var expectedStrings = expectedPlainString.IsEmpty() ? [] : expectedPlainString.Split(",");

            var resultStrings = inputString.SplitBySize(partsSize, false);

            foreach (var expectedString in expectedStrings)
            {
                resultStrings.Should().Contain(expectedString);
            }

            resultStrings.Should().HaveCount(expectedStrings.Length);
        }

        [Theory]
        [InlineData("12345", 6)]
        public void SplitBySize_PartSizeIsGreaterThanStringLength_ThrowsException(string inputString, int partSize)
        {
            Action action = () => inputString.SplitBySize(partSize);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
