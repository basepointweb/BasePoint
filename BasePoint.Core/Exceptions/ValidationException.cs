using BasePoint.Core.Extensions;
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

        public static void ThrowIf(bool condition, string message)
        {
            if (condition)
                throw new ValidationException(message);
        }

        public static void ThrowIfNullOrEmpty<T>(IEnumerable<T> enumerable, string message)
        {
            ThrowIf(enumerable.IsNullOrEmpty(), message);
        }

        public static void ThrowIfNull(object inputObject, string message)
        {
            ThrowIf(inputObject is null, message);
        }
    }
}