namespace TWBuildingAssistant.Data.Test
{
    using System.Linq;
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class JsonWeatherEffectsTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new JsonRepository<IWeatherEffect>();
            });
        }

        [Test]
        public void Any()
        {
            JsonRepository<IWeatherEffect> source = null;
            try
            {
                source = new JsonRepository<IWeatherEffect>();
            }
            catch
            {
                Assert.Inconclusive();
            }

            Assert.NotZero(source.DataSet.Count());
        }

        [Test]
        public void DuplicateKeys()
        {
            var source = this.GetRepository<IWeatherEffect>();
            var invalid = source.DataSet
                .GroupBy(x => new { x.ClimateId, x.WeatherId })
                .Where(x => x.Count() > 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidClimateIds()
        {
            var source = this.GetRepository<IWeatherEffect>();
            var invalid = source.DataSet
                .Where(x => x.ClimateId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentClimates()
        {
            var source = this.GetRepository<IWeatherEffect>();
            var climateSource = this.GetRepository<IClimate>();
            var invalid = source.DataSet
                .Where(x => !climateSource.DataSet.Any(y => y.Id == x.ClimateId))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidWeatherIds()
        {
            var source = this.GetRepository<IWeatherEffect>();
            var invalid = source.DataSet
                .Where(x => x.WeatherId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentWeathers()
        {
            var source = this.GetRepository<IWeatherEffect>();
            var weatherSource = this.GetRepository<IWeather>();
            var invalid = source.DataSet
                .Where(x => !weatherSource.DataSet.Any(y => y.Id == x.WeatherId))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidEffectIds()
        {
            var source = this.GetRepository<IWeatherEffect>();
            var invalid = source.DataSet
                .Where(x => x.ProvincialEffectId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentEffects()
        {
            var source = this.GetRepository<IWeatherEffect>();
            var effectSource = this.GetRepository<IProvincialEffect>();
            var invalid = source.DataSet
                .Where(x => !effectSource.DataSet.Any(y => y.Id == x.ProvincialEffectId))
                .ToList();

            Assert.Zero(invalid.Count);
        }
    }
}