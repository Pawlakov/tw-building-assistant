namespace Tests.Model.Effects
{
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Religions;

    /// <summary>
    /// A text fixture containing tests of the <see cref="InfluenceCalculator"/> class.
    /// </summary>
    [TestFixture]
    public class InfluenceCalculatorTests
    {
        /// <summary>
        /// A mock of a state religion.
        /// </summary>
        private IReligion stateReligion;

        /// <summary>
        /// A mock of a non-state religion.
        /// </summary>
        private IReligion otherReligion;

        /// <summary>
        /// The set up before all tests.
        /// </summary>
        [OneTimeSetUp]
        public void Preparation()
        {
            var tempState = new Mock<IReligion>();
            tempState.Setup(x => x.IsState).Returns(true);
            this.stateReligion = tempState.Object;
            var tempOther = new Mock<IReligion>();
            tempOther.Setup(x => x.IsState).Returns(false);
            this.otherReligion = tempOther.Object;
        }

        /// <summary>
        /// Checks whether the calculation of public order change caused by religions undergoes correctly.
        /// </summary>
        /// <param name="expectedPublicOrder">
        /// The expected Public Order.
        /// </param>
        /// <param name="stateInfluence">
        /// The state Influence.
        /// </param>
        /// <param name="otherInfluence">
        /// The other Influence.
        /// </param>
        [TestCase(-6, 1, 4)]
        [TestCase(-5, 7, 13)]
        [TestCase(-4, 2, 3)]
        [TestCase(-4, 1, 1)]
        [TestCase(-3, 3, 2)]
        [TestCase(-2, 3, 1)]
        [TestCase(-1, 4, 1)]
        public void Calculation(int expectedPublicOrder, int stateInfluence, int otherInfluence)
        {
            var state = new Mock<IInfluence>();
            var other = new Mock<IInfluence>();
            state.Setup(x => x.Value).Returns(stateInfluence);
            state.Setup(x => x.GetReligion()).Returns(this.stateReligion);
            other.Setup(x => x.Value).Returns(otherInfluence);
            other.Setup(x => x.GetReligion()).Returns(this.otherReligion);
            var influences = new List<IInfluence> { state.Object, other.Object };
            Assert.AreEqual(expectedPublicOrder, InfluenceCalculator.PublicOrder(influences), $"The {nameof(InfluenceCalculator.PublicOrder)} method returned an incorrect value.");
        }
    }
}