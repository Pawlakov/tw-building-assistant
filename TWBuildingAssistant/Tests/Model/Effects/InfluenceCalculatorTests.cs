namespace Tests.Model.Effects
{
    using System;
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Religions;
    
    [TestFixture]
    public class InfluenceCalculatorTests
    {
        private Mock<IInfluence> state;
        
        private Mock<IInfluence> other;

        private Mock<IInfluence> broken;

        [OneTimeSetUp]
        public void Preparation()
        {
            this.state = new Mock<IInfluence>();
            this.other = new Mock<IInfluence>();
            this.broken = new Mock<IInfluence>();

            var stateReligion = new Mock<IReligion>();
            var otherReligion = new Mock<IReligion>();

            stateReligion.Setup(x => x.IsState).Returns(true);
            otherReligion.Setup(x => x.IsState).Returns(false);
            
            this.state.Setup(x => x.Value).Returns(1);
            this.other.Setup(x => x.Value).Returns(1);
            this.broken.Setup(x => x.Value).Returns(1);
            this.state.Setup(x => x.GetReligion()).Returns(stateReligion.Object);
            this.other.Setup(x => x.GetReligion()).Returns(otherReligion.Object);
            this.broken.Setup(x => x.GetReligion()).Throws(new Exception("Literally anything."));
        }

        [TestCase(-6, 20.0)]
        [TestCase(-5, 35.0)]
        [TestCase(-4, 40.0)]
        [TestCase(-4, 50.0)]
        [TestCase(-3, 60.0)]
        [TestCase(-2, 75.0)]
        [TestCase(-1, 80.0)]
        [TestCase(0, 100.0)]
        public void PublicOrderCorrectTest(int expectedPublicOrder, double percentage)
        {
            Assert.AreEqual(expectedPublicOrder, InfluenceCalculator.PublicOrder(percentage), $"The {nameof(InfluenceCalculator.PublicOrder)} method returned an incorrect value.");
        }
        
        [Test]
        public void PublicOrderConsistencyTest([Values(0, 1, 3, 6, 8, 12)]int stateInfluence, [Values(0, 1, 2, 5, 9, 10)]int otherInfluence)
        {
            this.state.Setup(x => x.Value).Returns(stateInfluence);
            this.other.Setup(x => x.Value).Returns(otherInfluence);
            var influences = new List<IInfluence> { this.state.Object, this.other.Object };
            var percentage = InfluenceCalculator.Percentage(influences);
            Assert.AreEqual(InfluenceCalculator.PublicOrder(influences), InfluenceCalculator.PublicOrder(percentage), $"The {nameof(InfluenceCalculator.PublicOrder)} method returned an incorrect value.");
        }

        [TestCase(-5.0)]
        [TestCase(200.0)]
        public void PublicOrderCorrectTest(double percentage)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => InfluenceCalculator.PublicOrder(percentage), $"The {nameof(InfluenceCalculator.PublicOrder)} method returned an incorrect value.");
        }

        [TestCase(20.0, 1, 4)]
        [TestCase(35.0, 7, 13)]
        [TestCase(40.0, 2, 3)]
        [TestCase(50.0, 1, 1)]
        [TestCase(60.0, 3, 2)]
        [TestCase(75.0, 3, 1)]
        [TestCase(80.0, 4, 1)]
        [TestCase(100.0, 0, 0)]
        public void PercentageTest(double expectedPercentage, int stateInfluence, int otherInfluence)
        {
            this.state.Setup(x => x.Value).Returns(stateInfluence);
            this.other.Setup(x => x.Value).Returns(otherInfluence);
            var influences = new List<IInfluence> { this.state.Object, this.other.Object };
            Assert.AreEqual(expectedPercentage, InfluenceCalculator.Percentage(influences), 0.1, $"The {nameof(InfluenceCalculator.PublicOrder)} method returned an incorrect value.");
        }

        [Test]
        public void PercentageBrokenTest()
        {
            var influences = new List<IInfluence> { this.state.Object, this.other.Object, this.broken.Object };
            Assert.Throws<EffectsException>(() => InfluenceCalculator.Percentage(influences), $"The {nameof(InfluenceCalculator.PublicOrder)} method didn't throw {nameof(EffectsException)}.");
        }

        [Test]
        public void PercentageNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => InfluenceCalculator.Percentage(null), $"The {nameof(InfluenceCalculator.PublicOrder)} method didn't throw {nameof(ArgumentNullException)}.");
        }
    }
}