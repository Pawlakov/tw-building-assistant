namespace Tests.Model.Effects
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;

    [TestFixture]
    public class ProvincionalEffectTests
    {
        private ProvincialEffect filledEffect;

        private IEnumerable<IBonus> invalidBonuses;

        private IEnumerable<IBonus> validBonuses;

        private IEnumerable<IInfluence> invalidInfluences;

        private IEnumerable<IInfluence> validInfluences;

        [OneTimeSetUp]
        public void Preparation()
        {
            string dummyString;
            var invalidBonusMock = new Mock<IBonus>();
            invalidBonusMock.Setup(x => x.Validate(out dummyString)).Returns(false);
            var validBonusMock = new Mock<IBonus>();
            validBonusMock.Setup(x => x.Validate(out dummyString)).Returns(true);
            var invalidInfluenceMock = new Mock<IInfluence>();
            invalidInfluenceMock.Setup(x => x.Validate(out dummyString)).Returns(false);
            var validInfluenceMock = new Mock<IInfluence>();
            validInfluenceMock.Setup(x => x.Validate(out dummyString)).Returns(true);

            this.invalidBonuses = new List<IBonus> { validBonusMock.Object, validBonusMock.Object, validBonusMock.Object, invalidBonusMock.Object };
            this.validBonuses = new List<IBonus> { validBonusMock.Object, validBonusMock.Object, validBonusMock.Object, validBonusMock.Object };
            this.invalidInfluences = new List<IInfluence> { validInfluenceMock.Object, validInfluenceMock.Object, invalidInfluenceMock.Object };
            this.validInfluences = new List<IInfluence> { validInfluenceMock.Object, validInfluenceMock.Object, validInfluenceMock.Object };

            this.filledEffect =
            new ProvincialEffect()
            {
                Bonuses = this.validBonuses,
                Fertility = 1,
                FertilityDependentFood = 2,
                Growth = 3,
                Influences = this.validInfluences,
                RegularFood = 4,
                PublicOrder = 5,
                ResearchRate = 6,
                ReligiousOsmosis = 7,
                ProvincialSanitation = 8
            };
        }

        [TestCase(0, 0, 0, 0)]
        [TestCase(0, 0, 6, 0)]
        [TestCase(10, 0, 3, 10)]
        [TestCase(10, 10, 0, 10)]
        [TestCase(10, 10, 4, 50)]
        [TestCase(0, 50, 1, 50)]
        public void FoodCalcultaion(int regular, int fertilityDependent, int fertility, int expected)
        {
            var effect = new ProvincialEffect { RegularFood = regular, FertilityDependentFood = fertilityDependent };
            Assert.AreEqual(expected, effect.Food(fertility), $"The {nameof(ProvincialEffect.Food)} method returned an incorrect value.");
        }

        [Test]
        public void Validation([Values(true, false)]bool useValidBonuses, [Values(true, false)]bool useValidInfluences)
        {
            var effect = new ProvincialEffect
            {
                Bonuses = useValidBonuses ? this.validBonuses : this.invalidBonuses,
                Influences = useValidInfluences ? this.validInfluences : this.invalidInfluences
            };
            Assert.AreEqual(useValidInfluences && useValidBonuses, effect.Validate(out _), $"The {nameof(ProvincialEffect.Validate)} method returned an incorrect value.");
        }

        [Test]
        public void AggregationFilledWithEmpty([Values(true, false)]bool useEmpty)
        {
            var effect = this.filledEffect.Aggregate(useEmpty ? new ProvincialEffect() : null);
            Assert.AreEqual(this.filledEffect.Bonuses.Count(), effect.Bonuses.Count(), $"The result's {nameof(ProvincialEffect.Bonuses)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.Fertility, effect.Fertility, $"The result's {nameof(ProvincialEffect.Fertility)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.FertilityDependentFood, effect.FertilityDependentFood, $"The result's {nameof(ProvincialEffect.FertilityDependentFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Growth, effect.Growth, $"The result's {nameof(ProvincialEffect.Growth)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Influences.Count(), effect.Influences.Count(), $"The result's {nameof(ProvincialEffect.Influences)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.RegularFood, effect.RegularFood, $"The result's {nameof(ProvincialEffect.RegularFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.PublicOrder, effect.PublicOrder, $"The result's {nameof(ProvincialEffect.PublicOrder)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ResearchRate, effect.ResearchRate, $"The result's {nameof(ProvincialEffect.ResearchRate)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ReligiousOsmosis, effect.ReligiousOsmosis, $"The result's {nameof(ProvincialEffect.ReligiousOsmosis)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ProvincialSanitation, effect.ProvincialSanitation, $"The result's {nameof(ProvincialEffect.ProvincialSanitation)} contains an incorrect value.");
        }

        [Test]
        public void AggregationEmptyWithFilled()
        {
            var effect = new ProvincialEffect().Aggregate(this.filledEffect);
            Assert.AreEqual(this.filledEffect.Bonuses.Count(), effect.Bonuses.Count(), $"The result's {nameof(ProvincialEffect.Bonuses)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.Fertility, effect.Fertility, $"The result's {nameof(ProvincialEffect.Fertility)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.FertilityDependentFood, effect.FertilityDependentFood, $"The result's {nameof(ProvincialEffect.FertilityDependentFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Growth, effect.Growth, $"The result's {nameof(ProvincialEffect.Growth)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Influences.Count(), effect.Influences.Count(), $"The result's {nameof(ProvincialEffect.Influences)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.RegularFood, effect.RegularFood, $"The result's {nameof(ProvincialEffect.RegularFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.PublicOrder, effect.PublicOrder, $"The result's {nameof(ProvincialEffect.PublicOrder)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ResearchRate, effect.ResearchRate, $"The result's {nameof(ProvincialEffect.ResearchRate)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ReligiousOsmosis, effect.ReligiousOsmosis, $"The result's {nameof(ProvincialEffect.ReligiousOsmosis)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ProvincialSanitation, effect.ProvincialSanitation, $"The result's {nameof(ProvincialEffect.ProvincialSanitation)} contains an incorrect value.");
        }

        [Test]
        public void AggregationFilledWithFilled()
        {
            var effect = this.filledEffect.Aggregate(this.filledEffect);
            Assert.AreEqual(this.filledEffect.Bonuses.Count() * 2, effect.Bonuses.Count(), $"The result's {nameof(ProvincialEffect.Bonuses)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.Fertility * 2, effect.Fertility, $"The result's {nameof(ProvincialEffect.Fertility)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.FertilityDependentFood * 2, effect.FertilityDependentFood, $"The result's {nameof(ProvincialEffect.FertilityDependentFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Growth * 2, effect.Growth, $"The result's {nameof(ProvincialEffect.Growth)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Influences.Count() * 2, effect.Influences.Count(), $"The result's {nameof(ProvincialEffect.Influences)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.RegularFood * 2, effect.RegularFood, $"The result's {nameof(ProvincialEffect.RegularFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.PublicOrder * 2, effect.PublicOrder, $"The result's {nameof(ProvincialEffect.PublicOrder)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ResearchRate * 2, effect.ResearchRate, $"The result's {nameof(ProvincialEffect.ResearchRate)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ReligiousOsmosis * 2, effect.ReligiousOsmosis, $"The result's {nameof(ProvincialEffect.ReligiousOsmosis)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ProvincialSanitation * 2, effect.ProvincialSanitation, $"The result's {nameof(ProvincialEffect.ProvincialSanitation)} contains an incorrect value.");
        }
    }
}