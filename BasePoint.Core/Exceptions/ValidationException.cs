using BasePoint.Core.Shared;

namespace BasePoint.Core.Exceptions
{
    public class ValidationException : BaseException
    {
        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(IList<ErrorMessage> errors)
            : base(errors)
        {
        }

        public ValidationException(ErrorMessage errorMessage)
            : base(errorMessage)
        {
        }
    }
}