namespace Tests.Model.Effects
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Effects;

    /// <summary>
    /// A test fixture containing test of the <see cref="ProvincionalEffect"/> class.
    /// </summary>
    [TestFixture]
    public class ProvincionalEffectTests
    {
        /// <summary>
        /// An example <see cref="ProvincionalEffect"/> object filled with some values.
        /// </summary>
        private IProvincionalEffect filledEffect;

        /// <summary>
        /// A set of <see cref="IBonus"/> mocks. Not all of them are valid.
        /// </summary>
        private IEnumerable<IBonus> invalidBonuses;

        /// <summary>
        /// A set of <see cref="IBonus"/> mocks. All of them are valid.
        /// </summary>
        private IEnumerable<IBonus> validBonuses;

        /// <summary>
        /// A set of <see cref="IInfluence"/> mocks. Not all of them are valid.
        /// </summary>
        private IEnumerable<IInfluence> invalidInfluences;

        /// <summary>
        /// A set of <see cref="IInfluence"/> mocks. All of them are valid.
        /// </summary>
        private IEnumerable<IInfluence> validInfluences;

        /// <summary>
        /// The preparation required before any tests.
        /// </summary>
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
            new ProvincionalEffect()
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
            ProvincionalSanitation = 8
            };
        }

        /// <summary>
        /// Checks whether the calculation of total food effect undergoes correctly.
        /// </summary>
        /// <param name="regular">
        /// The food effect independent from fertility.
        /// </param>
        /// <param name="fertilityDependent">
        /// The food effect dependent on fertility.
        /// </param>
        /// <param name="fertility">
        /// The fertility level.
        /// </param>
        /// <param name="expected">
        /// The expected total food effect.
        /// </param>
        [TestCase(0, 0, 0, 0)]
        [TestCase(0, 0, 6, 0)]
        [TestCase(10, 0, 3, 10)]
        [TestCase(10, 10, 0, 10)]
        [TestCase(10, 10, 4, 50)]
        [TestCase(0, 50, 1, 50)]
        public void FoodCalcultaion(int regular, int fertilityDependent, int fertility, int expected)
        {
            IProvincionalEffect effect = new ProvincionalEffect { RegularFood = regular, FertilityDependentFood = fertilityDependent };
            Assert.AreEqual(expected, effect.Food(fertility), $"The {nameof(ProvincionalEffect.Food)} method returned an incorrect value.");
        }

        /// <summary>
        /// Checks whether the validation of <see cref="ProvincionalEffect"/> values undergoes correctly.
        /// </summary>
        /// <param name="useValidBonuses">
        /// Indicates whether the bonuses contained by the <see cref="ProvincionalEffect"/> should all be valid. Otherwise the bonuses will be invalid.
        /// </param>
        /// <param name="useValidInfluences">
        /// Indicates whether the influences contained by the <see cref="ProvincionalEffect"/> should all be valid. Otherwise the influences will be invalid.
        /// </param>
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Validation(bool useValidBonuses, bool useValidInfluences)
        {
            IProvincionalEffect effect = new ProvincionalEffect
                                         {
                                         Bonuses = useValidBonuses ? this.validBonuses : this.invalidBonuses,
                                         Influences = useValidInfluences ? this.validInfluences : this.invalidInfluences
                                         };
            Assert.AreEqual(useValidInfluences && useValidBonuses, effect.Validate(out _), $"The {nameof(ProvincionalEffect.Validate)} method returned an incorrect value.");
        }

        /// <summary>
        /// Checks whether the aggregation of a filled <see cref="ProvincionalEffect"/> with an empty one (or null) undergoes correctly.
        /// </summary>
        /// <param name="useEmpty">
        /// Indicates whether an empty <see cref="ProvincionalEffect"/> object will be used instead of null.
        /// </param>
        [TestCase(false)]
        [TestCase(true)]
        public void AggregationFilledWithEmpty(bool useEmpty)
        {
            IProvincionalEffect effect = this.filledEffect.Aggregate(useEmpty ? new ProvincionalEffect() : null);
            Assert.AreEqual(this.filledEffect.Bonuses.Count(), effect.Bonuses.Count(), $"The result's {nameof(ProvincionalEffect.Bonuses)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.Fertility, effect.Fertility, $"The result's {nameof(ProvincionalEffect.Fertility)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.FertilityDependentFood, effect.FertilityDependentFood, $"The result's {nameof(ProvincionalEffect.FertilityDependentFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Growth, effect.Growth, $"The result's {nameof(ProvincionalEffect.Growth)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Influences.Count(), effect.Influences.Count(), $"The result's {nameof(ProvincionalEffect.Influences)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.RegularFood, effect.RegularFood, $"The result's {nameof(ProvincionalEffect.RegularFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.PublicOrder, effect.PublicOrder, $"The result's {nameof(ProvincionalEffect.PublicOrder)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ResearchRate, effect.ResearchRate, $"The result's {nameof(ProvincionalEffect.ResearchRate)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ReligiousOsmosis, effect.ReligiousOsmosis, $"The result's {nameof(ProvincionalEffect.ReligiousOsmosis)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ProvincionalSanitation, effect.ProvincionalSanitation, $"The result's {nameof(ProvincionalEffect.ProvincionalSanitation)} contains an incorrect value.");
        }

        /// <summary>
        /// Checks whether the aggregation of an empty <see cref="ProvincionalEffect"/> with a filled one undergoes correctly.
        /// </summary>
        [Test]
        public void AggregationEmptyWithFilled()
        {
            IProvincionalEffect effect = new ProvincionalEffect().Aggregate(this.filledEffect);
            Assert.AreEqual(this.filledEffect.Bonuses.Count(), effect.Bonuses.Count(), $"The result's {nameof(ProvincionalEffect.Bonuses)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.Fertility, effect.Fertility, $"The result's {nameof(ProvincionalEffect.Fertility)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.FertilityDependentFood, effect.FertilityDependentFood, $"The result's {nameof(ProvincionalEffect.FertilityDependentFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Growth, effect.Growth, $"The result's {nameof(ProvincionalEffect.Growth)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Influences.Count(), effect.Influences.Count(), $"The result's {nameof(ProvincionalEffect.Influences)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.RegularFood, effect.RegularFood, $"The result's {nameof(ProvincionalEffect.RegularFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.PublicOrder, effect.PublicOrder, $"The result's {nameof(ProvincionalEffect.PublicOrder)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ResearchRate, effect.ResearchRate, $"The result's {nameof(ProvincionalEffect.ResearchRate)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ReligiousOsmosis, effect.ReligiousOsmosis, $"The result's {nameof(ProvincionalEffect.ReligiousOsmosis)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ProvincionalSanitation, effect.ProvincionalSanitation, $"The result's {nameof(ProvincionalEffect.ProvincionalSanitation)} contains an incorrect value.");
        }

        /// <summary>
        /// Checks whether the aggregation of a filled <see cref="ProvincionalEffect"/> with another filled one undergoes correctly.
        /// </summary>
        [Test]
        public void AggregationFilledWithFilled()
        {
            IProvincionalEffect effect = this.filledEffect.Aggregate(this.filledEffect);
            Assert.AreEqual(this.filledEffect.Bonuses.Count() * 2, effect.Bonuses.Count(), $"The result's {nameof(ProvincionalEffect.Bonuses)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.Fertility * 2, effect.Fertility, $"The result's {nameof(ProvincionalEffect.Fertility)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.FertilityDependentFood * 2, effect.FertilityDependentFood, $"The result's {nameof(ProvincionalEffect.FertilityDependentFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Growth * 2, effect.Growth, $"The result's {nameof(ProvincionalEffect.Growth)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.Influences.Count() * 2, effect.Influences.Count(), $"The result's {nameof(ProvincionalEffect.Influences)} contains an incorrect ammount of elements.");
            Assert.AreEqual(this.filledEffect.RegularFood * 2, effect.RegularFood, $"The result's {nameof(ProvincionalEffect.RegularFood)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.PublicOrder * 2, effect.PublicOrder, $"The result's {nameof(ProvincionalEffect.PublicOrder)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ResearchRate * 2, effect.ResearchRate, $"The result's {nameof(ProvincionalEffect.ResearchRate)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ReligiousOsmosis * 2, effect.ReligiousOsmosis, $"The result's {nameof(ProvincionalEffect.ReligiousOsmosis)} contains an incorrect value.");
            Assert.AreEqual(this.filledEffect.ProvincionalSanitation * 2, effect.ProvincionalSanitation, $"The result's {nameof(ProvincionalEffect.ProvincionalSanitation)} contains an incorrect value.");
        }
    }
}