namespace BasePoint.Core.Extensions
{
    public static class BooleanExtension
    {
        public static string ToBrazilianYesNo(this bool booleanValue)
        {
            return booleanValue ? "S" : "N";
        }

        public static string ToBrazilianYesNoEx(this bool booleanValue)
        {
            return booleanValue ? "Sim" : "Não";
        }

        public static bool FromBrazilianYesNo(this string yesNoValue)
        {
            return yesNoValue == "S";
        }

        public static bool FromBrazilianYesNoEx(this string yesNoValue)
        {
            return yesNoValue == "Sim";
        }

        public static string ToYesNo(this bool booleanValue)
        {
            return booleanValue ? "Y" : "N";
        }

        public static string ToYesNoEx(this bool booleanValue)
        {
            return booleanValue ? "Yes" : "No";
        }

        public static bool FromYesNo(this string yesNoValue)
        {
            return yesNoValue == "Y";
        }

        public static bool FromYesNoEx(this string yesNoValue)
        {
            return yesNoValue == "Yes";
        }
    }
}
