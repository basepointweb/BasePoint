using BasePoint.Core.Shared;

namespace BasePoint.Core.Exceptions
{
    public class CommandExecutionException : BaseException
    {
        public CommandExecutionException(ErrorMessage errorMessage) : base(errorMessage)
        {
        }
        public CommandExecutionException(string message) : base(message)
        {
        }

        public CommandExecutionException(IList<ErrorMessage> errors)
           : base(errors)
        {
        }
    }
}