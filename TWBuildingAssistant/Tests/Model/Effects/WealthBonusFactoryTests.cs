namespace Tests.Model.Effects
{
    using System;
    using System.Xml.Linq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;

    [TestFixture]
    public class WealthBonusFactoryTests
    {
        [TestCase("<multiplier c=\"All\">10</multiplier>", 10, WealthCategory.All, BonusType.Percentage)]
        [TestCase("<bonus c=\"Culture\">1000</bonus>", 1000, WealthCategory.Culture, BonusType.Simple)]
        [TestCase("<fertility_dependent c=\"Husbandry\">100</fertility_dependent>", 100, WealthCategory.Husbandry, BonusType.FertilityDependent)]
        public void Correct(string xml, int value, WealthCategory category, BonusType type)
        {
            var element = XDocument.Parse(xml).Root;
            var bonus = WealthBonusesFactory.MakeBonus(element);
            Assert.AreEqual(value, bonus.Value);
            Assert.AreEqual(category, bonus.Category);
            Assert.AreEqual(type, bonus.Type);
        }

        [TestCase("<multiplier c=\"All\">anything</multiplier>")]
        [TestCase("<multiplier c=\"Loss\">10</multiplier>")]
        [TestCase("<bonus>1000</bonus>")]
        [TestCase("<fertility_dependent c=\"Husbandry\" ayy=\"lmao\">100</fertility_dependent>")]
        [TestCase("<fertility_dependent ayy=\"lmao\">100</fertility_dependent>")]
        [TestCase("<train c=\"Culture\">1000</train>")]
        [TestCase("<bonus c=\"Maintenance\">100</bonus>")]
        public void Incorrect(string xml)
        {
            var element = XDocument.Parse(xml).Root;
            Assert.Throws<FormatException>(() => WealthBonusesFactory.MakeBonus(element));
        }
    }
}