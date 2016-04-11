using System.Collections.Generic;

namespace ProfanityFilter.Engine
{
    public interface INegativeWordSettingsProvider
    {
        List<string> GetAllWords();
        void TurnOnFilter();
        void TurnOffFilter();
        bool IsFilteringOn();
    }
}
