namespace Tests.Model.Effects
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;

    /// <summary>
    /// A test fixture containing tests of the <see cref="Bonus"/> class.
    /// </summary>
    [TestFixture]
    public class BonusTests
    {
        /// <summary>
        /// Checks whether the validation of <see cref="Bonus"/> values undergoes correctly.
        /// </summary>
        /// <param name="type">
        /// The type of the <see cref="Bonus"/>.
        /// </param>
        /// <param name="category">
        /// The category of the <see cref="Bonus"/>.
        /// </param>
        /// <param name="value">
        /// The value of the <see cref="Bonus"/>.
        /// </param>
        /// <param name="expectedResult">
        /// The expected result of verification.
        /// </param>
        [TestCase(BonusType.Simple, WealthCategory.Culture, 100, true)]
        [TestCase(BonusType.Simple, WealthCategory.All, 100, false)]
        [TestCase(BonusType.Simple, WealthCategory.Maintenance, -100, true)]
        [TestCase(BonusType.Simple, WealthCategory.LocalCommerce, -100, false)]
        [TestCase(BonusType.Simple, WealthCategory.Maintenance, 100, false)]
        [TestCase(BonusType.Percentage, WealthCategory.All, 10, true)]
        [TestCase(BonusType.Percentage, WealthCategory.All, -10, false)]
        [TestCase(BonusType.Percentage, WealthCategory.Maintenance, 10, false)]
        [TestCase(BonusType.FertilityDependent, WealthCategory.Agriculture, 100, true)]
        [TestCase(BonusType.FertilityDependent, WealthCategory.Husbandry, -100, false)]
        [TestCase(BonusType.FertilityDependent, WealthCategory.Subsistence, 100, false)]
        public void Validation(BonusType type, WealthCategory category, int value, bool expectedResult)
        {
            var subject = new Bonus { Category = category, Type = type, Value = value };
            Assert.AreEqual(expectedResult, subject.Validate(out _));
        }

        /// <summary>
        /// Checks whether the execution of a <see cref="Bonus"/> undergoes correctly when its category is not present in the dictionary.
        /// </summary>
        /// <param name="category">
        /// The category of the <see cref="Bonus"/>.
        /// </param>
        /// <param name="type">
        /// The type of the <see cref="Bonus"/>.
        /// </param>
        [TestCase(WealthCategory.All, BonusType.Percentage)]
        [TestCase(WealthCategory.Industry, BonusType.Simple)]
        [TestCase(WealthCategory.Agriculture, BonusType.FertilityDependent)]
        public void ExecutionNewCategory(WealthCategory category, BonusType type)
        {
            const int Value = 10;
            var dictionary = new Dictionary<WealthCategory, WealthRecord>();
            var bonus = new Bonus { Category = category, Type = type, Value = Value };
            bonus.Execute(dictionary);
            Assert.True(dictionary.ContainsKey(category));
            Assert.AreEqual(Value, dictionary[category][type]);
        }

        /// <summary>
        /// Checks whether the execution of a <see cref="Bonus"/> undergoes correctly when its category is already present in the dictionary.
        /// </summary>
        /// <param name="category">
        /// The category of the <see cref="Bonus"/>.
        /// </param>
        /// <param name="type">
        /// The type of the <see cref="Bonus"/>.
        /// </param>
        [TestCase(WealthCategory.All, BonusType.Percentage)]
        [TestCase(WealthCategory.Industry, BonusType.Simple)]
        [TestCase(WealthCategory.Agriculture, BonusType.FertilityDependent)]
        public void ExecutionExistingCategory(WealthCategory category, BonusType type)
        {
            const int Value = 10;
            var dictionary = new Dictionary<WealthCategory, WealthRecord> { { category, new WealthRecord() } };
            dictionary[category][type] = Value;
            var bonus = new Bonus { Category = category, Type = type, Value = Value };
            bonus.Execute(dictionary);
            Assert.True(dictionary.ContainsKey(category));
            Assert.AreEqual(Value + Value, dictionary[category][type]);
        }
    }
}