using System.Collections.Generic;

namespace ProfanityFilter.Engine
{
    //This class holds on to some status variables (may be not a good practice) but the reason is beacuse it's lacking any external persitance. So it's okay!
    public class NegativeWordSettingsProvider : INegativeWordSettingsProvider
    {
        private readonly List<string> words = new List<string>
        {
            "swine",
            "bad",
            "nasty",
            "horrible"
        };

        private bool doFiltering = true;

        public List<string> GetAllWords()
        {
            return words;
        }

        public void TurnOnFilter()
        {
            doFiltering = true;
        }

        public void TurnOffFilter()
        {
            doFiltering = false;
        }

        public bool IsFilteringOn()
        {
            return doFiltering;
        }
    }
}
