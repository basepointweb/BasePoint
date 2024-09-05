using BasePoint.Core.Shared;

namespace BasePoint.Core.Exceptions
{
    public class InvalidInputException : BaseException
    {
        public InvalidInputException(string message) : base(message)
        {
        }

        public InvalidInputException(IList<ErrorMessage> errors)
            : base(errors)
        {
        }

        public InvalidInputException(ErrorMessage errorMessage)
            : base(errorMessage)
        {
        }
    }
}
