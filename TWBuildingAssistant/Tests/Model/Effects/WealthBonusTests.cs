namespace Tests.Model.Effects
{
    using System;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;

    [TestFixture]
    public class WealthBonusTests
    {
        [TestCase(-50, BonusType.Simple, WealthCategory.Maintenance)]
        [TestCase(100, BonusType.FertilityDependent, WealthCategory.Husbandry)]
        [TestCase(5, BonusType.Percentage, WealthCategory.Industry)]
        public void Correct(int value, BonusType type, WealthCategory category)
        {
            var json = $"{{'Type': '{type}', 'Value': {value}, 'Category': '{category}'}}";
            var bonus = WealthBonus.Deserialize(json);
            Assert.AreEqual(value, bonus.Value);
            Assert.AreEqual(type, bonus.Type);
            Assert.AreEqual(category, bonus.Category);
        }

        [TestCase("{{'Value': {1}, 'Category': '{2}'}}")]
        [TestCase("{{'Type': '{0}', 'Category': '{2}'}}")]
        [TestCase("{{'Type': '{0}', 'Value': {1}}}")]
        public void MissingProperty(string format)
        {
            var json = string.Format(format, BonusType.Percentage, 10, WealthCategory.All);
            Assert.Throws<ArgumentNullException>(() => WealthBonus.Deserialize(json));
        }

        [TestCase(100, BonusType.Simple, WealthCategory.All)]
        [TestCase(100, BonusType.Simple, WealthCategory.Maintenance)]
        [TestCase(-100, BonusType.Simple, WealthCategory.MaritimeCommerce)]
        [TestCase(10, BonusType.Percentage, WealthCategory.Maintenance)]
        [TestCase(-10, BonusType.Percentage, WealthCategory.Subsistence)]
        [TestCase(100, BonusType.FertilityDependent, WealthCategory.Culture)]
        [TestCase(-100, BonusType.FertilityDependent, WealthCategory.Husbandry)]
        public void InvalidCombinationOfValues(int value, BonusType type, WealthCategory category)
        {
            var json = $"{{'Type': '{type}', 'Value': {value}, 'Category': '{category}'}}";
            Assert.Throws<ArgumentOutOfRangeException>(() => WealthBonus.Deserialize(json));
        }

        [Test]
        public void ExcessProperty()
        {
            var json = $"{{'Type': '{BonusType.Percentage}', 'Value': {10}, 'Category': '{WealthCategory.All}', 'Excess': 'ScrewYou'}}";
            Assert.Throws<JsonSerializationException>(() => WealthBonus.Deserialize(json));
        }

        [Test]
        public void CompleteNonsense()
        {
            const string json = "{This ,is:: a'nything' but cor}rec}{t Json.";
            Assert.Throws<JsonReaderException>(() => WealthBonus.Deserialize(json));
        }
    }
}