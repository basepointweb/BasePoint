using BasePoint.Core.Shared;

namespace BasePoint.Core.Exceptions
{
    public class ResourceNotFoundException : BaseException
    {
        public ResourceNotFoundException(string message) : base(message)
        {

        }
        public ResourceNotFoundException(IList<ErrorMessage> errors)
            : base(errors)
        {
        }

        public ResourceNotFoundException(ErrorMessage errorMessage)
            : base(errorMessage)
        {

        }
    }
}