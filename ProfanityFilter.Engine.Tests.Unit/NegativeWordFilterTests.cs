using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace ProfanityFilter.Engine.Tests.Unit
{
    [TestFixture]
    public class NegativeWordFilterTests
    {
        [TestCase("The weather in Manchester in winter is bad. It rains all the time - it must be horrible for people visiting.", 2, "bad", "horrible")]
        [TestCase("The weather in Manchester in winter is bad, It rains all the time - it must be horrible for people visiting.", 2, "bad", "horrible")]
        [TestCase("horrible, pleasant", 1, "bad", "horrible")]
        [TestCase("horrible, horrible", 2, "bad", "horrible")]
        [TestCase("a,b,c", 2, "a", "c")]
        [TestCase("all good words", 0, "bad", "horrible")]
        public void Count_WhenInvokedWithNonEmptyPhrase_Returns_NegativeWordsCount(string phrase, int expectedCount, params string[] negativeWords)
        {
            var negativeWordSettingsProvider = Mock.Of<INegativeWordSettingsProvider>();
            Mock.Get(negativeWordSettingsProvider)
                .Setup(x => x.GetAllWords())
                .Returns(negativeWords.ToList());

            var sut = new NegativeWordFilter(negativeWordSettingsProvider);
            var actualCount = sut.Count(phrase);
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestCase("Horrible, Pleasant", 1, "bAd", "HorRible")]
        [TestCase("horrible, horriblE", 2, "baD", "hOrribLe")]
        [TestCase("a,B,c", 2, "a", "C")]
        public void Count_WhenInvokedWithNonEmptyPhrase_Returns_NegativeWordsCount_CaseInsensitive(string phrase, int expectedCount, params string[] negativeWords)
        {
            var negativeWordSettingsProvider = Mock.Of<INegativeWordSettingsProvider>();
            Mock.Get(negativeWordSettingsProvider)
                .Setup(x => x.GetAllWords())
                .Returns(negativeWords.ToList());

            var sut = new NegativeWordFilter(negativeWordSettingsProvider);
            var actualCount = sut.Count(phrase);
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestCase("", "bad", "horrible")]
        [TestCase("   ", "bad", "horrible")]
        [TestCase(null, "bad", "horrible")]
        public void Count_WhenInvokedWithEmptyOrNullPhrase_Returns_Zero(string phrase, params string[] negativeWords)
        {
            var negativeWordSettingsProvider = Mock.Of<INegativeWordSettingsProvider>();
            Mock.Get(negativeWordSettingsProvider)
                .Setup(x => x.GetAllWords())
                .Returns(negativeWords.ToList());

            var sut = new NegativeWordFilter(negativeWordSettingsProvider);
            var actualCount = sut.Count(phrase);
            Assert.AreEqual(0, actualCount);
        }

        [TestCase("Horrible, pleasant")]
        [TestCase("Horrible horrible")]
        [TestCase("all good words")]
        [TestCase("a b c")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public void Count_WhenThereAreNoNegativeWords_Returns_Zero(string phrase)
        {
            var negativeWordSettingsProvider = Mock.Of<INegativeWordSettingsProvider>();
            Mock.Get(negativeWordSettingsProvider)
                .Setup(x => x.GetAllWords())
                .Returns(new List<string>());

            var sut = new NegativeWordFilter(negativeWordSettingsProvider);
            var actualCount = sut.Count(phrase);
            Assert.AreEqual(0, actualCount);
        }

        [TestCase("The weather in Manchester in winter is bad. It rains all the time - it must be horrible for people visiting.",
            "The weather in Manchester in winter is b#d. It rains all the time - it must be h######e for people visiting.",
            "bad", "horrible")]
        [TestCase("The weather in Manchester in winter is bad, It rains all the time - it must be HorRible for people visiting.",
            "The weather in Manchester in winter is b#d, It rains all the time - it must be H######e for people visiting.",
            "bad", "horrible")]
        [TestCase("horrible, pleasant",
            "h######e, pleasant",
            "bad", "horrible")]
        [TestCase("horriblE, hOrrible",
            "h######E, h######e",
            "bad", "horrible")]
        [TestCase("all good words",
            "all good words",
            "bad", "horrible")]
        public void Filter_WhenFilteringIsOn_And_InvokedWithNonEmptyPhrase_Returns_FilteredPhrase(string phrase, string expected, params string[] negativeWords)
        {
            var negativeWordSettingsProvider = Mock.Of<INegativeWordSettingsProvider>();
            Mock.Get(negativeWordSettingsProvider)
                .Setup(x => x.GetAllWords())
                .Returns(negativeWords.ToList());
            Mock.Get(negativeWordSettingsProvider)
                .Setup(x => x.IsFilteringOn())
                .Returns(true);

            var sut = new NegativeWordFilter(negativeWordSettingsProvider);
            var actual = sut.Filter(phrase);

            Assert.AreEqual(expected, actual);
        }

        [TestCase("", "bad", "horrible")]
        [TestCase("   ", "bad", "horrible")]
        [TestCase(null, "bad", "horrible")]
        public void Filter_WhenInvokedWithEmptyOrNullPhrase_Returns_TheSame(string phrase, params string[] negativeWords)
        {
            var negativeWordSettingsProvider = Mock.Of<INegativeWordSettingsProvider>();
            Mock.Get(negativeWordSettingsProvider)
                .Setup(x => x.GetAllWords())
                .Returns(negativeWords.ToList());
            Mock.Get(negativeWordSettingsProvider)
                .Setup(x => x.IsFilteringOn())
                .Returns(true);

            var sut = new NegativeWordFilter(negativeWordSettingsProvider);
            var actual = sut.Filter(phrase);
            Assert.AreEqual(phrase, actual);
        }

        [TestCase("Horrible, pleasant")]
        [TestCase("Horrible horrible")]
        [TestCase("all good words")]
        [TestCase("a b c")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public void Filter_WhenThereAreNoNegativeWords_Returns_TheSame(string phrase)
        {
            var negativeWordSettingsProvider = Mock.Of<INegativeWordSettingsProvider>();
            Mock.Get(negativeWordSettingsProvider)
                .Setup(x => x.GetAllWords())
                .Returns(new List<string>());
            Mock.Get(negativeWordSettingsProvider)
                .Setup(x => x.IsFilteringOn())
                .Returns(true);

            var sut = new NegativeWordFilter(negativeWordSettingsProvider);
            var actual = sut.Filter(phrase);
            Assert.AreEqual(phrase, actual);
        }

        [TestCase("Horrible, pleasant", "bad", "horrible")]
        [TestCase("Horrible horrible", "bad", "horrible")]
        [TestCase("all good words", "bad", "horrible")]
        [TestCase("a b c", "bad", "horrible")]
        [TestCase("", "bad", "horrible")]
        [TestCase("     ", "bad", "horrible")]
        [TestCase(null)]
        public void Filter_WhenFilteringIsOff_Returns_TheSame(string phrase, params string[] negativeWords)
        {
            var negativeWordSettingsProvider = Mock.Of<INegativeWordSettingsProvider>();
            Mock.Get(negativeWordSettingsProvider)
                .Setup(x => x.GetAllWords())
                .Returns(negativeWords.ToList());
            Mock.Get(negativeWordSettingsProvider)
                .Setup(x => x.IsFilteringOn())
                .Returns(false);

            var sut = new NegativeWordFilter(negativeWordSettingsProvider);
            var actual = sut.Filter(phrase);
            Assert.AreEqual(phrase, actual);
        }

        [Test]
        public void TurnOnFilter_Invokes_TurnOnFilter_On_NegativeWordSettingsProvider()
        {
            var negativeWordSettingsProvider = Mock.Of<INegativeWordSettingsProvider>();

            var sut = new NegativeWordFilter(negativeWordSettingsProvider);
            sut.TurnOnFilter();

            Mock.Get(negativeWordSettingsProvider)
                .Verify(x => x.TurnOnFilter(), Times.Once);
        }

        [Test]
        public void TurnOffFilter_Invokes_TurnOffFilter_On_NegativeWordSettingsProvider()
        {
            var negativeWordSettingsProvider = Mock.Of<INegativeWordSettingsProvider>();

            var sut = new NegativeWordFilter(negativeWordSettingsProvider);
            sut.TurnOffFilter();

            Mock.Get(negativeWordSettingsProvider)
                .Verify(x => x.TurnOffFilter(), Times.Once);
        }
    }
}
