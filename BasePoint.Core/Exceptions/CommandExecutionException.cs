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

        public static void ThrowIf(bool condition, string message)
        {
            if (condition)
                throw new CommandExecutionException(message);
        }
    }
}