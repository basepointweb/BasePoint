using BasePoint.Core.Shared;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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

        public static string CapitalizeEachWordFirstLetter(this string text)
        {
            var wordSeparators = new char[] { Constants.CharSpace, Constants.CharTab, Constants.CharEnter };

            var phrase = text.Split(wordSeparators, StringSplitOptions.RemoveEmptyEntries);

            for (var wordIndex = Constants.ZeroBasedFirstIndex; wordIndex < phrase.Length; wordIndex++)
            {
                phrase[wordIndex] = CapitalizeFirstLetter(phrase[wordIndex]);
            }

            return string.Join(Constants.CharSpace, phrase);
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

        public static string[] SubstringsBetween(this string str, string delimiterString)
        {
            return SubstringsBetween(str, delimiterString, delimiterString);
        }

        public static string[] SubstringsBetween(this string str, string initialString, string finalString)
        {
            var substrings = new List<string>();

            string pattern = $@"(?<=\{initialString})(.*?)(?=\{finalString})";

            MatchCollection matches = Regex.Matches(str, pattern);

            foreach (Match match in matches)
            {
                substrings.Add(match.Groups[1].Value);
            }

            return substrings.ToArray();
        }
    }
}