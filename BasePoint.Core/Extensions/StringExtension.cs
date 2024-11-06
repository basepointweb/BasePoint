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

            foreach (var character in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(character);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string FirstWord(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var words = text.Split(Constants.StringSpace);

            var result = string.Empty;

            if (words.Any())
                result = words[Constants.QuantityZero];

            return result;
        }

        public static string LastWord(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var words = text.Split(Constants.StringSpace);

            var result = string.Empty;

            if (words.Length != Constants.QuantityZero)
            {
                var lastWord = words.Last();

                if (lastWord != text.LastWord())
                    result = lastWord;
            }

            return result;
        }

        public static string CapitalizeFirstLetter(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return
                text.Substring(Constants.ZeroBasedFirstIndex, Constants.QuantityOne).ToUpper() +
                text.Substring(Constants.QuantityOne)
                .ToLower();
        }

        public static int WordCount(this string text)
        {
            var wordSeparators = new char[] { Constants.CharSpace, Constants.CharTab, Constants.CharEnter };

            int wordCount = text.Split(wordSeparators, StringSplitOptions.RemoveEmptyEntries)
                .Length;

            return wordCount;
        }
    }
}