
namespace BasePoint.Core.Shared
{
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            var errorMessage = ConvertMessageToErrorMessage(message);

            Code = errorMessage.Code;
            Message = errorMessage.Message;
        }
        public ErrorMessage(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public static ErrorMessage ConvertMessageToErrorMessage(string message)
        {
            var code = Constants.ErrorCodes.DefaultErrorCode;
            var errorMessage = message;

            if (message.Contains(Constants.ErrorMessageSeparator))
            {
                var messageParts = message.Split(Constants.ErrorMessageSeparator);

                code = messageParts[0];
                errorMessage = messageParts[1];
            }

            return new ErrorMessage(code, errorMessage);
        }

        public static string GetErrorCodeFromMessage(string message)
        {
            var errorMessage = ConvertMessageToErrorMessage(message);

            return errorMessage.Code;
        }
        public string Code { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return Code + Constants.ErrorMessageSeparator + Message;
        }
    }
}