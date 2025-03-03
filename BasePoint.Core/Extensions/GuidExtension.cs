namespace BasePoint.Core.Extensions
{
    public static class GuidExtension
    {
        public static bool IsValid(this Guid? value)
        {
            return (value != null) && IsValid(value.Value);
        }

        public static bool IsValid(this Guid value)
        {
            return (!IsEmpty(value));
        }

        public static bool IsEmpty(this Guid value)
        {
            return (value == Guid.Empty);
        }
    }
}
