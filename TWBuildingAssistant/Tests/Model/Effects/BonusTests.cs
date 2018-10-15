namespace Tests.Model.Effects
{
    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;
    
    [TestFixture]
    public class BonusTests
    {
        [TestCase(BonusType.Simple, WealthCategory.Culture, 100)]
        [TestCase(BonusType.Simple, WealthCategory.Maintenance, -100)]
        [TestCase(BonusType.Percentage, WealthCategory.All, 10)]
        [TestCase(BonusType.FertilityDependent, WealthCategory.Agriculture, 100)]
        public void ValidateValidTest(BonusType type, WealthCategory category, int value)
        {
            var subject = new Bonus { Category = category, Type = type, Value = value };
            Assert.AreEqual(true, subject.Validate(out _), $"The {nameof(Bonus.Validate)} method returned an incorrect value.");
        }

        [TestCase(BonusType.Simple, WealthCategory.Culture, 0)]
        [TestCase(BonusType.Simple, WealthCategory.Maintenance, 0)]
        [TestCase(BonusType.Percentage, WealthCategory.All, 0)]
        [TestCase(BonusType.FertilityDependent, WealthCategory.Agriculture, 0)]
        [TestCase(BonusType.Simple, WealthCategory.All, 100)]
        [TestCase(BonusType.Simple, WealthCategory.LocalCommerce, -100)]
        [TestCase(BonusType.Simple, WealthCategory.Maintenance, 100)]
        [TestCase(BonusType.Percentage, WealthCategory.All, -10)]
        [TestCase(BonusType.Percentage, WealthCategory.Maintenance, 10)]
        [TestCase(BonusType.FertilityDependent, WealthCategory.Husbandry, -100)]
        [TestCase(BonusType.FertilityDependent, WealthCategory.Subsistence, 100)]
        public void ValidateInvalidTest(BonusType type, WealthCategory category, int value)
        {
            var subject = new Bonus { Category = category, Type = type, Value = value };
            Assert.AreEqual(false, subject.Validate(out _), $"The {nameof(Bonus.Validate)} method returned an incorrect value.");
        }
    }
}