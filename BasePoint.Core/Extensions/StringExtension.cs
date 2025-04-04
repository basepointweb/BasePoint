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

        public static bool EqualsIgnoreCase(this string inputString, string other)
        {
            return inputString.Equals(other, StringComparison.OrdinalIgnoreCase);
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

            string escapedInitial = Regex.Escape(initialString);
            string escapedFinal = Regex.Escape(finalString);

            string pattern = $@"(?<={escapedInitial})(.*?)(?={escapedFinal})";

            MatchCollection matches = Regex.Matches(str, pattern);

            foreach (Match match in matches)
            {
                substrings.Add(match.Value);
            }

            return [.. substrings];
        }


        public static string[] SplitBySize(this string str, int partsSize, bool smallerPartInTheEndWhenIsOddCharsNumber = true)
        {
            var parts = new List<string>();

            if (partsSize <= Constants.QuantityZero || str.IsEmpty())
                return [.. parts];

            ArgumentOutOfRangeException.ThrowIfLessThan(str.Length, partsSize, "The size must be less than or equals to string length");

            var remainingString = str;

            if (smallerPartInTheEndWhenIsOddCharsNumber)
            {
                while (!remainingString.IsEmpty())
                {
                    var subStringSize = partsSize > remainingString.Length ? remainingString.Length : partsSize;

                    var subString = remainingString.Substring(Constants.ZeroBasedFirstIndex, subStringSize);

                    parts.Add(subString);

                    remainingString = remainingString.Substring(subStringSize);
                }
            }
            else
            {
                while (!remainingString.IsEmpty())
                {
                    var subStringSize = partsSize > remainingString.Length ? remainingString.Length : partsSize;

                    var subString = remainingString.Substring(remainingString.Length - subStringSize);

                    parts.Insert(Constants.ZeroBasedFirstIndex, subString);

                    remainingString = remainingString.Substring(Constants.ZeroBasedFirstIndex, remainingString.Length - subStringSize);
                }
            }

            return [.. parts];
        }

        public static int ParseSubstringAsInt(this string str, int startIndex)
        {
            var stringValue = str.Substring(startIndex);

            int.TryParse(stringValue, out int result);

            return result;
        }

        public static int ParseSubstringAsInt(this string str, int startIndex, int length)
        {
            var stringValue = str.Substring(startIndex, length);

            int.TryParse(stringValue, out int result);

            return result;
        }

        public static decimal ParseSubstringAsDecimal(this string str, int startIndex, int length)
        {
            var stringValue = str.Substring(startIndex, length);

            decimal.TryParse(stringValue, out decimal result);

            return result;
        }

        public static decimal ParseSubstringAsDecimal(this string str, int startIndex)
        {
            var stringValue = str.Substring(startIndex);

            decimal.TryParse(stringValue, out decimal result);

            return result;
        }

        public static DateTime ParseSubstringAsDateTime(this string str, int startIndex, int length)
        {
            var stringValue = str.Substring(startIndex, length);

            DateTime.TryParse(stringValue, out DateTime result);

            return result;
        }

        public static DateTime ParseSubstringAsDateTime(this string str, int startIndex)
        {
            var stringValue = str.Substring(startIndex);

            DateTime.TryParse(stringValue, out DateTime result);

            return result;
        }
    }
}