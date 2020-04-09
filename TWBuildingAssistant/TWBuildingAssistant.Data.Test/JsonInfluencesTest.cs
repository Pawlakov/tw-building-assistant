namespace TWBuildingAssistant.Data.Test
{
    using System.Linq;
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class JsonInfluencesTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new JsonRepository<IInfluence>();
            });
        }

        [Test]
        public void Any()
        {
            JsonRepository<IInfluence> source = null;
            try
            {
                source = new JsonRepository<IInfluence>();
            }
            catch
            {
                Assert.Inconclusive();
            }

            Assert.NotZero(source.DataSet.Count());
        }

        [Test]
        public void InvalidValue()
        {
            var source = this.GetRepository<IInfluence>();
            var invalid = source.DataSet
                .Where(x => x.Value < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentReligion()
        {
            var source = this.GetRepository<IInfluence>();
            var religionSource = this.GetRepository<IReligion>();
            var invalid = source.DataSet
                .Where(x => x.ReligionId.HasValue)
                .Where(x => !religionSource.DataSet.Any(y => y.Id == x.ReligionId))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void MissingEffect()
        {
            var source = this.GetRepository<IInfluence>();
            var invalid = source.DataSet
                .Where(x => x.ProvincialEffectId == null && x.RegionalEffectId == null)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void ConflictingEffect()
        {
            var source = this.GetRepository<IInfluence>();
            var invalid = source.DataSet
                .Where(x => x.ProvincialEffectId != null && x.RegionalEffectId != null)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentProvincialEffect()
        {
            var source = this.GetRepository<IInfluence>();
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
            var source = this.GetRepository<IInfluence>();
            var effectSource = this.GetRepository<IRegionalEffect>();
            var invalid = source.DataSet
                .Where(x => x.RegionalEffectId.HasValue)
                .Where(x => !effectSource.DataSet.Any(y => y.Id == x.RegionalEffectId))
                .ToList();

            Assert.Zero(invalid.Count);
        }
    }
}