using System;
using ProfanityFilter.Engine;

namespace ProfanityFilter.Application
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var filter = new NegativeWordFilter(new NegativeWordSettingsProvider());

            do
            {
                Console.WriteLine("Press");
                Console.WriteLine("1 to Get Content");
                Console.WriteLine("2 to Disable Filtering");
                Console.WriteLine("3 to Enable Filtering");
                Console.WriteLine("ANY other key to exit.");
                var keyPressed = Console.ReadKey().KeyChar;

                switch (keyPressed)
                {
                    case '1':
                        DisplayContent(filter); break;
                    case '2':
                        DisableFiltering(filter); break;
                    case '3':
                        EnableFiltering(filter); break;
                    default:
                        return;
                }

                Console.WriteLine("");
                Console.WriteLine("");
            } while (true);
        }

        private static void DisplayContent(NegativeWordFilter filter)
        {
            string content =
                "The weather in Manchester in winter is bad. It rains all the time - it must be horrible for people visiting.";

            Console.WriteLine("");
            Console.WriteLine("Scanned the text:");
            Console.WriteLine(filter.Filter(content));
            Console.WriteLine("Total Number of negative words: " + filter.Count(content));
        }

        private static void EnableFiltering(NegativeWordFilter filter)
        {
            filter.TurnOnFilter();
            Console.WriteLine("");
            Console.WriteLine("Filtering Enabled");
        }

        private static void DisableFiltering(NegativeWordFilter filter)
        {
            filter.TurnOffFilter();
            Console.WriteLine("");
            Console.WriteLine("Filtering Disabled");
        }
    }
}
