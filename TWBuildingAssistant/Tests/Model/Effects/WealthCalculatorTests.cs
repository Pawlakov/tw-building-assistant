namespace Tests.Model.Effects
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;

    /// <summary>
    /// A text fixture containing tests of the <see cref="WealthCalculator"/> class.
    /// </summary>
    [TestFixture]
    public class WealthCalculatorTests
    {
        /// <summary>
        /// The expected result of wealth calculation.
        /// </summary>
        private const double ExpectedResult = 1000.0;

        /// <summary>
        /// The fertility required for the wealth calculation.
        /// </summary>
        private const int Fertility = 2;

        /// <summary>
        /// A prepared set of bonuses for the test.
        /// </summary>
        private IEnumerable<IBonus> bonuses;

        /// <summary>
        /// The preparation required before the test.
        /// </summary>
        [OneTimeSetUp]
        public void Preparation()
        {
            this.bonuses =
            new List<IBonus>
            {
            new Bonus
            {
            Category = WealthCategory.All,
            Type = BonusType.Percentage,
            Value = 5
            },
            new Bonus
            {
            Category =
            WealthCategory.Subsistence,
            Type = BonusType.Simple,
            Value = 1000
            },
            new Bonus
            {
            Category =
            WealthCategory.Husbandry,
            Type = BonusType
            .FertilityDependent,
            Value = 50
            },
            new Bonus
            {
            Category =
            WealthCategory.Husbandry,
            Type = BonusType.Simple,
            Value = 100
            },
            new Bonus
            {
            Category =
            WealthCategory.Husbandry,
            Type = BonusType.Percentage,
            Value = 10
            },
            new Bonus
            {
            Category =
            WealthCategory.Maintenance,
            Type = BonusType.Simple,
            Value = -280
            }
            };
        }

        /// <summary>
        /// Checks whether the calculation od province's total wealth undergoes correctly.
        /// </summary>
        [Test]
        public void Calculation()
        {
            Assert.AreEqual(ExpectedResult, WealthCalculator.CalculateTotalWealth(this.bonuses, Fertility), 0.1);
        }
    }
}
