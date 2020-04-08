namespace TWBuildingAssistant.Data.Test
{
    using System.Linq;
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class JsonProvincesTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new ProvinceRepository();
            });
        }

        [Test]
        public void Any()
        {
            ProvinceRepository source = null;
            try
            {
                source = new ProvinceRepository();
            }
            catch
            {
                Assert.Inconclusive();
            }

            Assert.NotZero(source.DataSet.Count);
        }

        [Test]
        public void InvalidIds()
        {
            var source = this.GetRepository<IProvince>();
            var invalid = source.DataSet
                .Where(x => x.Id < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NullNames()
        {
            var source = this.GetRepository<IProvince>();
            var invalid = source.DataSet
                .Where(x => x.Name == null)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void EmptyNames()
        {
            var source = this.GetRepository<IProvince>();
            var invalid = source.DataSet
                .Where(x => x.Name == string.Empty)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void DuplicateIds()
        {
            var source = this.GetRepository<IProvince>();
            var invalid = source.DataSet
                .GroupBy(x => x.Id)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void DuplicateNames()
        {
            var source = this.GetRepository<IProvince>();
            var invalid = source.DataSet
                .GroupBy(x => x.Name)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidFertilities()
        {
            var source = this.GetRepository<IProvince>();
            var invalid = source.DataSet
                .Where(x => x.Fertility < 1 || x.Fertility > 6)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidClimateIds()
        {
            var source = this.GetRepository<IProvince>();
            var invalid = source.DataSet
                .Where(x => x.ClimateId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentClimates()
        {
            var source = this.GetRepository<IProvince>();
            var climateSource = this.GetRepository<IClimate>();
            var invalid = source.DataSet
                .Where(x => !climateSource.DataSet.Any(y => y.Id == x.ClimateId))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidEffectIds()
        {
            var source = this.GetRepository<IProvince>();
            var invalid = source.DataSet
                .Where(x => x.ProvincialEffectId.HasValue)
                .Where(x => x.ProvincialEffectId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentEffects()
        {
            var source = this.GetRepository<IProvince>();
            var effectSource = this.GetRepository<IProvincialEffect>();
            var invalid = source.DataSet
                .Where(x => x.ProvincialEffectId.HasValue)
                .Where(x => !effectSource.DataSet.Any(y => y.Id == x.ProvincialEffectId))
                .ToList();

            Assert.Zero(invalid.Count);
        }
    }
}