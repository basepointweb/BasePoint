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

        public static bool AnyTrue(params bool[] values)
        {
            var anyTrue = values.Any(x => x);

            return anyTrue;
        }

        public static bool AnyFalse(params bool[] values)
        {
            var anyTrue = values.Any(x => !x);

            return anyTrue;
        }

        public static bool AllTrue(params bool[] values)
        {
            var falseItems = values.Where(x => x);

            return falseItems.Count() == values.Length;
        }

        public static bool AllFalse(params bool[] values)
        {
            var falseItems = values.Where(x => !x);

            return falseItems.Count() == values.Length;
        }
    }
}
