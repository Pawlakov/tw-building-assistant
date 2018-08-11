namespace Tests.Model.Effects
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;

    /// <summary>
    /// A test fixture containing test of the <see cref="RegionalEffect"/> class.
    /// </summary>
    public class RegionalEffectTests
    {
        /// <summary>
        /// An example <see cref="RegionalEffect"/> object filled with some values.
        /// </summary>
        private IRegionalEffect filledEffect;

        /// <summary>
        /// The preparation required before any tests.
        /// </summary>
        [OneTimeSetUp]
        public void Preparation()
        {
            string dummyString;
            var validBonusMock = new Mock<IBonus>();
            validBonusMock.Setup(x => x.Validate(out dummyString)).Returns(true);
            var validInfluenceMock = new Mock<IInfluence>();
            validInfluenceMock.Setup(x => x.Validate(out dummyString)).Returns(true);

            var validBonuses = new List<IBonus> { validBonusMock.Object, validBonusMock.Object, validBonusMock.Object, validBonusMock.Object };
            var validInfluences = new List<IInfluence> { validInfluenceMock.Object, validInfluenceMock.Object, validInfluenceMock.Object };

            this.filledEffect =
            new RegionalEffect()
            {
            Bonuses = validBonuses,
            Fertility = 1,
            FertilityDependentFood = 2,
            Growth = 3,
            Influences = validInfluences,
            RegularFood = 4,
            PublicOrder = 5,
            ResearchRate = 6,
            ReligiousOsmosis = 7,
            ProvincionalSanitation = 8,
            RegionalSanitation = 9
            };
        }

        /// <summary>
        /// Checks whether the aggregation of a filled <see cref="RegionalEffect"/> with an empty one (or null) undergoes correctly.
        /// </summary>
        /// <param name="useEmpty">
        /// Indicates whether an empty <see cref="RegionalEffect"/> object will be used instead of null.
        /// </param>
        [TestCase(false)]
        [TestCase(true)]
        public void AggregationFilledWithEmpty(bool useEmpty)
        {
            IRegionalEffect effect = this.filledEffect.Aggregate(useEmpty ? new RegionalEffect() : null);
            Assert.AreEqual(this.filledEffect.Bonuses.Count(), effect.Bonuses.Count(), $"The result's {nameof(RegionalEffect.Bonuses)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.Fertility, effect.Fertility, $"The result's {nameof(RegionalEffect.Fertility)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.FertilityDependentFood, effect.FertilityDependentFood, $"The result's {nameof(RegionalEffect.FertilityDependentFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Growth, effect.Growth, $"The result's {nameof(RegionalEffect.Growth)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Influences.Count(), effect.Influences.Count(), $"The result's {nameof(RegionalEffect.Influences)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.RegularFood, effect.RegularFood, $"The result's {nameof(RegionalEffect.RegularFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.PublicOrder, effect.PublicOrder, $"The result's {nameof(RegionalEffect.PublicOrder)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ResearchRate, effect.ResearchRate, $"The result's {nameof(RegionalEffect.ResearchRate)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ReligiousOsmosis, effect.ReligiousOsmosis, $"The result's {nameof(RegionalEffect.ReligiousOsmosis)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ProvincionalSanitation, effect.ProvincionalSanitation, $"The result's {nameof(RegionalEffect.ProvincionalSanitation)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.RegionalSanitation, effect.RegionalSanitation, $"The result's {nameof(RegionalEffect.RegionalSanitation)} contains an incorrect value.");
        }

        /// <summary>
        /// Checks whether the aggregation of an empty <see cref="RegionalEffect"/> with a filled one undergoes correctly.
        /// </summary>
        [Test]
        public void AggregationEmptyWithFilled()
        {
            IRegionalEffect effect = new RegionalEffect().Aggregate(this.filledEffect);
            Assert.AreEqual(this.filledEffect.Bonuses.Count(), effect.Bonuses.Count(), $"The result's {nameof(RegionalEffect.Bonuses)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.Fertility, effect.Fertility, $"The result's {nameof(RegionalEffect.Fertility)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.FertilityDependentFood, effect.FertilityDependentFood, $"The result's {nameof(RegionalEffect.FertilityDependentFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Growth, effect.Growth, $"The result's {nameof(RegionalEffect.Growth)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Influences.Count(), effect.Influences.Count(), $"The result's {nameof(RegionalEffect.Influences)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.RegularFood, effect.RegularFood, $"The result's {nameof(RegionalEffect.RegularFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.PublicOrder, effect.PublicOrder, $"The result's {nameof(RegionalEffect.PublicOrder)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ResearchRate, effect.ResearchRate, $"The result's {nameof(RegionalEffect.ResearchRate)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ReligiousOsmosis, effect.ReligiousOsmosis, $"The result's {nameof(RegionalEffect.ReligiousOsmosis)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ProvincionalSanitation, effect.ProvincionalSanitation, $"The result's {nameof(RegionalEffect.ProvincionalSanitation)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.RegionalSanitation, effect.RegionalSanitation, $"The result's {nameof(RegionalEffect.RegionalSanitation)} contains an incorrect value.");
        }

        /// <summary>
        /// Checks whether the aggregation of a filled <see cref="RegionalEffect"/> with another filled one undergoes correctly.
        /// </summary>
        [Test]
        public void AggregationFilledWithFilled()
        {
            IRegionalEffect effect = this.filledEffect.Aggregate(this.filledEffect);
            Assert.AreEqual(this.filledEffect.Bonuses.Count() * 2, effect.Bonuses.Count(), $"The result's {nameof(RegionalEffect.Bonuses)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.Fertility * 2, effect.Fertility, $"The result's {nameof(RegionalEffect.Fertility)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.FertilityDependentFood * 2, effect.FertilityDependentFood, $"The result's {nameof(RegionalEffect.FertilityDependentFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Growth * 2, effect.Growth, $"The result's {nameof(RegionalEffect.Growth)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Influences.Count() * 2, effect.Influences.Count(), $"The result's {nameof(RegionalEffect.Influences)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.RegularFood * 2, effect.RegularFood, $"The result's {nameof(RegionalEffect.RegularFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.PublicOrder * 2, effect.PublicOrder, $"The result's {nameof(RegionalEffect.PublicOrder)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ResearchRate * 2, effect.ResearchRate, $"The result's {nameof(RegionalEffect.ResearchRate)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ReligiousOsmosis * 2, effect.ReligiousOsmosis, $"The result's {nameof(RegionalEffect.ReligiousOsmosis)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ProvincionalSanitation * 2, effect.ProvincionalSanitation, $"The result's {nameof(RegionalEffect.ProvincionalSanitation)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.RegionalSanitation * 2, effect.RegionalSanitation, $"The result's {nameof(RegionalEffect.RegionalSanitation)} contains an incorrect value.");
        }
    }
}