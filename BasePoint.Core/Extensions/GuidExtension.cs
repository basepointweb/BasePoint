namespace BasePoint.Core.Extensions
{
    public static class GuidExtension
    {
        public static bool IsValid(this Guid? value)
        {
            return (value != null) && (value != Guid.Empty);
        }

        public static bool IsValid(this Guid value)
        {
            return (value != Guid.Empty);
        }
    }
}
