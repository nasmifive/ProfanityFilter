using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ProfanityFilter.Engine
{
    public class NegativeWordFilter
    {
        private const char MASK = '#';
        private readonly INegativeWordSettingsProvider negativeWordSettingsProvider;

        public NegativeWordFilter(INegativeWordSettingsProvider negativeWordSettingsProvider)
        {
            this.negativeWordSettingsProvider = negativeWordSettingsProvider;
        }

        public int Count(string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase))
                return 0;

            var negativeWordRegexes =
                negativeWordSettingsProvider.GetAllWords()
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(negWord => new Regex(negWord, RegexOptions.IgnoreCase));

            return
                negativeWordRegexes.Sum(regex => regex.Matches(phrase).Count);
        }

        public string Filter(string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase) || !negativeWordSettingsProvider.IsFilteringOn())
                return phrase;

            var regexes =
                negativeWordSettingsProvider.GetAllWords()
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(negWord => new Regex(negWord, RegexOptions.IgnoreCase));

            var sb = new StringBuilder(phrase);
            foreach (var regex in regexes)
            {
                var matches =
                    regex.Matches(phrase).OfType<Match>().Select(m => m.ToString());

                foreach (var match in matches)
                    sb = sb.Replace(match, MaskMiddle(match, MASK));
            }

            return sb.ToString();
        }

        public void TurnOnFilter()
        {
            negativeWordSettingsProvider.TurnOnFilter();
        }

        public void TurnOffFilter()
        {
            negativeWordSettingsProvider.TurnOffFilter();
        }

        private string MaskMiddle(string str, char mask)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var start = 0;
            var end = str.Length - 1;
            var maskedChars =
                str.Select((chr, index) => index == start || index == end ? chr : mask).ToArray();

            return
                new string(maskedChars);
        }
    }
}
