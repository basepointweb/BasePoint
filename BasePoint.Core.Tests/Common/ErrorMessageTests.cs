using BasePoint.Core.Shared;
using FluentAssertions;
using Xunit;

namespace BasePoint.Core.Tests.Common
{
    public class ErrorMessageTests
    {
        public ErrorMessageTests()
        {
        }

        [Fact]
        public void Constructor_GivenAStringMessageWithoutCode_ReturnsErrorMessage()
        {
            //Act
            var exception = new ErrorMessage("Error message test");

            // Assert
            exception.Code.Should().Be(Constants.ErrorCodes.DefaultErrorCode);
            exception.Message.Should().Be("Error message test");
        }

        [Fact]
        public void Constructor_GivenAStringMessageWithoutSeparatorAndCode_ReturnsErrorMessage()
        {
            //Act
            var exception = new ErrorMessage($"999{Constants.ErrorMessageSeparator}Error message test");

            // Assert
            exception.Code.Should().Be("999");
            exception.Message.Should().Be("Error message test");
        }

        [Fact]
        public void Constructor_GivenCodeAndMessage_ReturnsErrorMessage()
        {
            //Act
            var exception = new ErrorMessage("999", "Error message test");

            // Assert
            exception.Code.Should().Be("999");
            exception.Message.Should().Be("Error message test");
        }

        [Fact]
        public void GetErrorCodeFromMessage_GivenMessageWithCodeAndSeparator_ReturnsErrorCode()
        {
            //Act
            var errorCode = ErrorMessage.GetErrorCodeFromMessage("999;Error message test");

            // Assert
            errorCode.Should().Be("999");
        }
    }
}