namespace Tests.Model.Effects
{
    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Religions;

    /// <summary>
    /// A test fixture containing tests of the <see cref="Influence"/> class.
    /// </summary>
    [TestFixture]
    public class InfluenceTests
    {
        /// <summary>
        /// The mock of <see cref="IReligion"/> used in some tests.
        /// </summary>
        private readonly IReligion mockReligion = new Mock<IReligion>().Object;

        /// <summary>
        /// Checks whether the validation of <see cref="Influence"/> values undergoes correctly.
        /// </summary>
        /// <param name="value">
        /// The value of the <see cref="Influence"/>.
        /// </param>
        /// <param name="useReligion">
        /// Indicates whether the <see cref="Influence"/> should use either the mock or null.
        /// </param>
        /// <param name="expectedResult">
        /// The expected result of the validation.
        /// </param>
        [TestCase(0, false, false)]
        [TestCase(-10, false, false)]
        [TestCase(10, false, true)]
        [TestCase(0, true, false)]
        [TestCase(-10, true, false)]
        [TestCase(10, true, true)]
        public void Validation(int value, bool useReligion, bool expectedResult)
        {
            var religion = useReligion ? this.mockReligion : null;
            var influence = new Influence { ReligionId = religion?.Id, Value = value };
            Assert.AreEqual(expectedResult, influence.Validate(out _), $"The {nameof(Influence.Validate)} method returned an incorrect value.");
        }
    }
}