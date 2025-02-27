using BasePoint.Core.Extensions;
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

        public static void ThrowIfNullOrEmpty<T>(IEnumerable<T> enumerable, string message)
        {
            ThrowIf(enumerable.IsNullOrEmpty(), message);
        }

        public static void ThrowIfNotEmpty<T>(IEnumerable<T> enumerable, string message)
        {
            ThrowIf(!enumerable.IsNullOrEmpty(), message);
        }
    }
}