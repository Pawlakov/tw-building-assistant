namespace TWBuildingAssistant.Data.Test
{
    using System.Linq;
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class JsonBonusesTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new BonusRepository();
            });
        }

        [Test]
        public void Any()
        {
            BonusRepository source = null;
            try
            {
                source = new BonusRepository();
            }
            catch
            {
                Assert.Inconclusive();
            }

            Assert.NotZero(source.DataSet.Count);
        }

        [Test]
        public void NoValue()
        {
            var source = this.GetRepository<IBonus>();
            var invalid = source.DataSet
                .Where(x => x.Value == 0)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void SimpleToAll()
        {
            var source = this.GetRepository<IBonus>();
            var invalid = source.DataSet
                .Where(x => x.Type == BonusType.Simple)
                .Where(x => x.Category == IncomeCategory.All)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void SimpleNegative()
        {
            var source = this.GetRepository<IBonus>();
            var invalid = source.DataSet
                .Where(x => x.Type == BonusType.Simple)
                .Where(x => x.Category != IncomeCategory.Maintenance)
                .Where(x => x.Value < 0)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void SimplePositiveMaintenance()
        {
            var source = this.GetRepository<IBonus>();
            var invalid = source.DataSet
                .Where(x => x.Type == BonusType.Simple)
                .Where(x => x.Category == IncomeCategory.Maintenance)
                .Where(x => x.Value > 0)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void PercentageNegative()
        {
            var source = this.GetRepository<IBonus>();
            var invalid = source.DataSet
                .Where(x => x.Type == BonusType.Percentage)
                .Where(x => x.Value < 0)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void PercentageMaintenance()
        {
            var source = this.GetRepository<IBonus>();
            var invalid = source.DataSet
                .Where(x => x.Type == BonusType.Percentage)
                .Where(x => x.Category == IncomeCategory.Maintenance)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void FertilityDependentNegative()
        {
            var source = this.GetRepository<IBonus>();
            var invalid = source.DataSet
                .Where(x => x.Type == BonusType.FertilityDependent)
                .Where(x => x.Value < 0)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void FertilityDependentWrongCategory()
        {
            var source = this.GetRepository<IBonus>();
            var invalid = source.DataSet
                .Where(x => x.Type == BonusType.FertilityDependent)
                .Where(x => x.Category != IncomeCategory.Agriculture && x.Category != IncomeCategory.Husbandry)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void MissingEffect()
        {
            var source = this.GetRepository<IBonus>();
            var invalid = source.DataSet
                .Where(x => x.ProvincialEffectId == null && x.RegionalEffectId == null)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void ConflictingEffect()
        {
            var source = this.GetRepository<IBonus>();
            var invalid = source.DataSet
                .Where(x => x.ProvincialEffectId != null && x.RegionalEffectId != null)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentProvincialEffect()
        {
            var source = this.GetRepository<IBonus>();
            var effectSource = this.GetRepository<IProvincialEffect>();
            var invalid = source.DataSet
                .Where(x => x.ProvincialEffectId.HasValue)
                .Where(x => !effectSource.DataSet.Any(y => y.Id == x.ProvincialEffectId))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentRegionalEffect()
        {
            var source = this.GetRepository<IBonus>();
            var effectSource = this.GetRepository<IRegionalEffect>();
            var invalid = source.DataSet
                .Where(x => x.RegionalEffectId.HasValue)
                .Where(x => !effectSource.DataSet.Any(y => y.Id == x.RegionalEffectId))
                .ToList();

            Assert.Zero(invalid.Count);
        }
    }
}