namespace Tests.Model.Effects
{
    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;

    [TestFixture]
    public class WealthRecordTests
    {
        [TestCase(100, 0, 0, 0, 100.0)]
        [TestCase(100, 10, 0, 0, 110.0)]
        [TestCase(100, 20, 10, 0, 120.0)]
        [TestCase(100, 10, 10, 5, 165.0)]
        [TestCase(0, 10, 10, 3, 33.0)]
        [TestCase(0, 10, 0, 0, 0.0)]
        public void Calculation(int baseValue, int percents, int valuePerFertilityLevel, int fertility, double expectedValue)
        {
            var subject = new WealthRecord
                              {
                                  [BonusType.Simple] = baseValue,
                                  [BonusType.Percentage] = percents,
                                  [BonusType.FertilityDependent] = valuePerFertilityLevel
                              };
            Assert.AreEqual(expectedValue, subject.Calculate(fertility), 0.1, $"The {nameof(WealthRecord.Calculate)} method returned an unexpected value.");
        }
    }
}