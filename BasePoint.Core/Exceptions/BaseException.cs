using BasePoint.Core.Shared;
using System.Text;

namespace BasePoint.Core.Exceptions
{
    public abstract class BaseException : Exception
    {
        public IList<ErrorMessage> Errors { get; }
        public BaseException(string message)
            : this(new ErrorMessage(message))
        {
        }

        public BaseException(ErrorMessage errorMessage)
            : base(errorMessage.ToString())
        {
            Errors = new List<ErrorMessage>() { errorMessage };
        }

        public BaseException(IList<ErrorMessage> errors)
            : base(ErrorsToErrorMessage(errors))
        {
            Errors = errors;
        }

        protected static string ErrorsToErrorMessage(IList<ErrorMessage> errors)
        {
            var message = new StringBuilder();

            for (int i = Constants.ZeroBasedFirstIndex; i < errors.Count; i++)
            {
                var errorMessage = errors[i];

                if (i < errors.Count - 1)
                    message.AppendLine(errorMessage.ToString());
                else
                    message.Append(errorMessage.ToString());
            }
            return message.ToString();
        }
    }
}