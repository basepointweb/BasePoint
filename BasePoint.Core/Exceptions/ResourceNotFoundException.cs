using BasePoint.Core.Extensions;
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

        public static void ThrowIf(bool condition, string message)
        {
            if (condition)
                throw new ResourceNotFoundException(message);
        }

        public static void ThrowIfNullOrEmpty<T>(IEnumerable<T> enumerable, string message)
        {
            ThrowIf(enumerable.IsNullOrEmpty(), message);
        }

        public static void ThrowIfNull(object inputObject, string message)
        {
            ThrowIf(inputObject is null, message);
        }
        public static void ThrowIfAnyNull<T>(IEnumerable<T> enumerable, string message)
        {
            ThrowIf(enumerable.AnyNull(), message);
        }

        public static void ThrowIfAllNull<T>(IEnumerable<T> enumerable, string message)
        {
            ThrowIf(enumerable.AllNull(), message);
        }
    }
}