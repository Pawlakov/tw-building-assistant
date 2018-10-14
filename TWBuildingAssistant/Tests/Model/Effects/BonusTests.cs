namespace Tests.Model.Effects
{
    using System;
    using System.Collections.Generic;

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

        [TestCase(WealthCategory.All, BonusType.Percentage)]
        [TestCase(WealthCategory.Industry, BonusType.Simple)]
        [TestCase(WealthCategory.Agriculture, BonusType.FertilityDependent)]
        public void ExecuteNewCategoryTest(WealthCategory category, BonusType type)
        {
            const int Value = 10;
            var dictionary = new Dictionary<WealthCategory, WealthRecord>();
            var bonus = new Bonus { Category = category, Type = type, Value = Value };
            Assert.DoesNotThrow(() => bonus.Execute(dictionary), $"The {nameof(Bonus.Execute)} method failed.");
            Assert.True(dictionary.ContainsKey(category), "The dictionary does not contain an element that should have been there.");
            Assert.AreEqual(Value, dictionary[category][type], "The value within the newly added record is incorrect.");
        }

        [TestCase(WealthCategory.All, BonusType.Percentage)]
        [TestCase(WealthCategory.Industry, BonusType.Simple)]
        [TestCase(WealthCategory.Agriculture, BonusType.FertilityDependent)]
        public void ExecuteExistingCategoryTest(WealthCategory category, BonusType type)
        {
            const int Value = 10;
            var dictionary = new Dictionary<WealthCategory, WealthRecord> { { category, new WealthRecord() } };
            dictionary[category][type] = Value;
            var bonus = new Bonus { Category = category, Type = type, Value = Value };
            Assert.DoesNotThrow(() => bonus.Execute(dictionary), $"The {nameof(Bonus.Execute)} method failed.");
            Assert.AreEqual(Value + Value, dictionary[category][type], "The value within the corresponding record is incorrect.");
        }

        [Test]
        public void ExecutionNullCategoriesTest()
        {
            var bonus = new Bonus { Category = WealthCategory.All, Type = BonusType.Percentage, Value = 10 };
            Assert.Throws<ArgumentNullException>(() => bonus.Execute(null), $"The {nameof(Bonus.Execute)} method didn't throw {nameof(ArgumentNullException)}.");
        }

        [Test]
        public void ExecutionBrokenCategoryTest()
        {
            var dictionary = new Dictionary<WealthCategory, WealthRecord> { { WealthCategory.All, null } };
            var bonus = new Bonus { Category = WealthCategory.All, Type = BonusType.Percentage, Value = 10 };
            Assert.Throws<EffectsException>(() => bonus.Execute(dictionary), $"The {nameof(Bonus.Execute)} method didn't throw {nameof(EffectsException)}.");
        }
    }
}