namespace Tests.Model.Effects
{
    using System;

    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model;
    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Religions;
    
    [TestFixture]
    public class InfluenceTests
    {
        private Mock<IParser<IReligion>> parser;
        private Mock<IReligion> religion;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.religion = new Mock<IReligion>();
            this.religion.Setup(x => x.Name).Returns("1");

            this.parser = new Mock<IParser<IReligion>>();
            this.parser.Setup(x => x.Find(It.IsAny<int?>())).Returns<int?>(
                x =>
                    {
                        return x == 1 ? this.religion.Object : null;
                    });
        }

        [TestCase(10, null)]
        [TestCase(10, 1)]
        public void ValidateValidTest(int value, int? religionId)
        {
            var influence = new Influence
                                {
                                    ReligionId = religionId,
                                    Value = value,
                                };
            influence.SetReligionParser(this.parser.Object);
            Assert.AreEqual(true, influence.Validate(out _), $"The {nameof(Influence.Validate)} method returned false.");
        }

        [TestCase(0, null, true)]
        [TestCase(-10, null, true)]
        [TestCase(0, 1, true)]
        [TestCase(-10, 1, true)]
        [TestCase(10, null, false)]
        [TestCase(10, 1, false)]
        [TestCase(10, 0, true)]
        public void ValidateInvalidTest(int value, int? religionId, bool useParser)
        {
            var influence = new Influence
                                {
                                    ReligionId = religionId,
                                    Value = value
                                };
            if (useParser)
            {
                influence.SetReligionParser(this.parser.Object);
            }

            Assert.AreEqual(false, influence.Validate(out _), $"The {nameof(Influence.Validate)} method returned true.");
        }

        [Test]
        public void GetReligionConcreteTest()
        {
            var influence = new Influence
                                {
                                    ReligionId = 1,
                                    Value = 10,
                                };
            influence.SetReligionParser(this.parser.Object);
            IReligion parsed = null;
            Assert.DoesNotThrow(() => parsed = influence.GetReligion(), $"The {nameof(Influence.GetReligion)} method failed.");
            Assert.AreEqual(this.religion.Object, parsed, $"Received wrong {nameof(IReligion)} object.");
        }

        [Test]
        public void GetReligionStateTest()
        {
            var influence = new Influence
                                {
                                    ReligionId = null,
                                    Value = 10,
                                };
            influence.SetReligionParser(this.parser.Object);
            IReligion parsed = null;
            Assert.DoesNotThrow(() => parsed = influence.GetReligion(), $"The {nameof(Influence.GetReligion)} method failed.");
            Assert.IsNull(parsed, $"Received wrong {nameof(IReligion)} object.");
        }

        [Test]
        public void GetReligionNullParserTest()
        {
            var influence = new Influence
                                {
                                    ReligionId = 1,
                                    Value = 10,
                                };
            Assert.Throws<ArgumentNullException>(() => influence.SetReligionParser(null), $"{nameof(ArgumentNullException)} was not thrown.");
        }

        [Test]
        public void GetReligionNotSetParserTest()
        {
            var influence = new Influence
                                {
                                    ReligionId = 1,
                                    Value = 10,
                                };
            Assert.Throws<EffectsException>(() => influence.GetReligion(), $"{nameof(EffectsException)} was not thrown.");
        }

        [Test]
        public void GetReligionNonexistentTest()
        {
            var influence = new Influence
                                {
                                    ReligionId = 2,
                                    Value = 10
                                };
            Assert.DoesNotThrow(() => influence.SetReligionParser(this.parser.Object), $"The {nameof(Influence.GetReligion)} method failed.");
            Assert.Throws<EffectsException>(() => influence.GetReligion(), $"{nameof(EffectsException)} was not thrown.");
        }
    }
}