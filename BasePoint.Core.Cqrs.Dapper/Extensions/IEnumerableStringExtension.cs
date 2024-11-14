using Dapper;

namespace BasePoint.Core.Cqrs.Dapper.Extensions
{
    public static class IEnumerableStringExtension
    {
        public static IEnumerable<DbString> GetAnsiStrings(this IEnumerable<string> inputStrings, int? size = null)
        {
            foreach (var inputString in inputStrings)
            {
                var dbString = new DbString { IsAnsi = true, Value = inputString };

                if (size.HasValue)
                    dbString.Length = size.Value;

                yield return dbString;
            }
        }
    }
}
