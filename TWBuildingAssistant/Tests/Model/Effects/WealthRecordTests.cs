namespace Tests.Model.Effects
{
    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;

    /// <summary>
    /// A test fixture containing tests of the <see cref="WealthRecord"/> class.
    /// </summary>
    [TestFixture]
    public class WealthRecordTests
    {
        /// <summary>
        /// Checks whether the calculation of wealth within one category undergoes correctly.
        /// </summary>
        /// <param name="baseValue">
        /// The total value of <see cref="BonusType.Simple"/> bonuses.
        /// </param>
        /// <param name="percents">
        /// The total value of <see cref="BonusType.Percentage"/> bonuses.
        /// </param>
        /// <param name="valuePerFertilityLevel">
        /// The total value of <see cref="BonusType.FertilityDependent"/> bonuses.
        /// </param>
        /// <param name="fertility">
        /// The fertility level.
        /// </param>
        /// <param name="expectedValue">
        /// The expected total value.
        /// </param>
        [TestCase(100, 0, 0, 0, 100.0)]
        [TestCase(100, 10, 0, 0, 110.0)]
        [TestCase(100, 20, 10, 0, 120.0)]
        [TestCase(100, 10, 10, 5, 165.0)]
        [TestCase(0, 10, 10, 3, 33.0)]
        [TestCase(0, 10, 0, 0, 0.0)]
        public void Calculation(int baseValue, int percents, int valuePerFertilityLevel, int fertility, double expectedValue)
        {
            var subject = new WealthRecord();
            subject[BonusType.Simple] = baseValue;
            subject[BonusType.Percentage] = percents;
            subject[BonusType.FertilityDependent] = valuePerFertilityLevel;
            Assert.AreEqual(expectedValue, subject.Calculate(fertility), 0.1);
        }
    }
}