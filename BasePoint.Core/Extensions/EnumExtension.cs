using System.ComponentModel;
using System.Reflection;

namespace BasePoint.Core.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum enumValue)
        {
            Type type = enumValue.GetType();
            string name = Enum.GetName(type, enumValue);

            if (!name.IsEmpty())
            {
                FieldInfo field = type.GetField(name);

                if (field is not null && Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute descriptionAttribute)
                    return descriptionAttribute.Description;
            }

            return string.Empty;
        }
    }
}
