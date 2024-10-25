using BasePoint.Core.Shared;
using System.Globalization;
using System.Text;

namespace BasePoint.Core.Extensions
{
    public static class StringExtension
    {
        public static string Format(this string inputString, params object[] values)
        {
            return string.Format(inputString, values);
        }

        public static bool IsEmpty(this string inputString)
        {
            return string.IsNullOrWhiteSpace(inputString);
        }

        public static string RemoveAccents(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string FirstWord(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            var words = name.Split(" ");

            var result = string.Empty;

            if (words.Any())
                result = words[0];

            return result;
        }

        public static string LastWord(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            var words = name.Split(Constants.StringSpace);

            var result = string.Empty;

            if (words.Length != Constants.QuantityZeroItems)
            {
                var lastWord = words.Last();

                if (lastWord != name.LastWord())
                    result = lastWord;
            }

            return result;
        }

        public static string CapitalizeFirstLetter(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            return name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
        }
    }
}