using BasePoint.Core.Extensions;
using BasePoint.Core.Shared;
using System.Linq.Expressions;

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

        public static void ThrowIfNotEmpty<T>(IEnumerable<T> enumerable, string message)
        {
            ThrowIf(!enumerable.IsNullOrEmpty(), message);
        }

        public static void ThrowIfHasDuplicates<T, TKey>(IEnumerable<T> enumerable, string message, params Expression<Func<T, TKey>>[] keySelectors)
        {
            ThrowIf(enumerable.HasDuplicates(keySelectors), message);
        }

        public static void ThrowIfNull(object inputObject, string message)
        {
            ThrowIf(inputObject is null, message);
        }

        public static void ThrowIfNotNull(object inputObject, string message)
        {
            ThrowIf(inputObject is not null, message);
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