using FluentValidation.Results;

namespace BasePoint.Core.Shared
{
    public class ValidationFailureBuilder
    {
        public ValidationFailure Build(string validationMessage)
        {
            return ConvertMessageToErrorMessage(validationMessage);
        }

        private static ValidationFailure ConvertMessageToErrorMessage(string message)
        {
            var code = Constants.ErrorCodes.DefaultErrorCode;
            var errorMessage = message;

            if (message.Contains(Constants.ErrorMessageSeparator))
            {
                var messageParts = message.Split(Constants.ErrorMessageSeparator);

                code = messageParts[0];
                errorMessage = messageParts[1];
            }

            return new ValidationFailure()
            {
                ErrorCode = code,
                ErrorMessage = errorMessage
            };
        }
    }
}
