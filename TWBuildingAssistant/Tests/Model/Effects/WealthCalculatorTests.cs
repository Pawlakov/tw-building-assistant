namespace Tests.Model.Effects
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;
    
    [TestFixture]
    public class WealthCalculatorTests
    {
        private const double ExpectedResult = 1000.0;
        
        private const int Fertility = 2;
        
        private IEnumerable<IBonus> bonuses;
        
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
            Type = BonusType.FertilityDependent,
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
        
        [Test]
        public void Calculation()
        {
            Assert.AreEqual(ExpectedResult, WealthCalculator.CalculateTotalWealth(this.bonuses, Fertility), 0.1, $"The {nameof(WealthCalculator.CalculateTotalWealth)} method returned an incorrect value.");
        }
    }
}
