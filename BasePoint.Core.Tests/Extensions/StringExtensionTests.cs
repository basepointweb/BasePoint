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
        public void SubstringsBetween_Should_ReturListOfStrings(string inputString, string expectedPlainString)
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
        public void SubstringsBetween_Should_ReturListOfStrings2(string inputString, string expectedPlainString)
        {
            var expectedStrings = expectedPlainString.IsEmpty() ? [] : expectedPlainString.Split(",");

            var resultStrings = inputString.SubstringsBetween("#", "#");

            foreach (var expectedString in expectedStrings)
            {
                resultStrings.Should().Contain(expectedString);
            }

            resultStrings.Should().HaveCount(expectedStrings.Length);
        }
    }
}
