namespace BasePoint.Core.Extensions
{
    public static class TypeExtension
    {
        public static Type GetRealType(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);

            return underlyingType ?? type;
        }
    }
}