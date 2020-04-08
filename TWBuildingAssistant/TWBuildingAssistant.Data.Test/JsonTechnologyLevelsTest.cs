namespace TWBuildingAssistant.Data.Test
{
    using System.Linq;
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class JsonTechnologyLevelsTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new TechnologyLevelRepository();
            });
        }

        [Test]
        public void Any()
        {
            TechnologyLevelRepository source = null;
            try
            {
                source = new TechnologyLevelRepository();
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
            var source = this.GetRepository<ITechnologyLevel>();
            var invalid = source.DataSet
                .Where(x => x.Id < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void DuplicateIds()
        {
            var source = this.GetRepository<ITechnologyLevel>();
            var invalid = source.DataSet
                .GroupBy(x => x.Id)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidOrders()
        {
            var source = this.GetRepository<ITechnologyLevel>();
            var invalid = source.DataSet
                .Where(x => x.Order < 1 && x.Order > 4)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void DuplicateOrders()
        {
            var source = this.GetRepository<ITechnologyLevel>();
            var invalid = source.DataSet
                .GroupBy(x => new { x.FactionId, x.Order })
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void GroupedOrders()
        {
            var source = this.GetRepository<ITechnologyLevel>();
            var invalid = source.DataSet
                .GroupBy(x => x.FactionId)
                .Where(x => x.Count() != 4)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidFactionIds()
        {
            var source = this.GetRepository<ITechnologyLevel>();
            var invalid = source.DataSet
                .Where(x => x.FactionId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentFactions()
        {
            var source = this.GetRepository<ITechnologyLevel>();
            var factionSource = this.GetRepository<IFaction>();
            var invalid = source.DataSet
                .Where(x => !factionSource.DataSet.Any(y => y.Id == x.FactionId))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidUniversalEffectIds()
        {
            var source = this.GetRepository<ITechnologyLevel>();
            var invalid = source.DataSet
                .Where(x => x.UniversalProvincialEffectId.HasValue)
                .Where(x => x.UniversalProvincialEffectId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentUniversalEffects()
        {
            var source = this.GetRepository<ITechnologyLevel>();
            var effectSource = this.GetRepository<IProvincialEffect>();
            var invalid = source.DataSet
                .Where(x => x.UniversalProvincialEffectId.HasValue)
                .Where(x => !effectSource.DataSet.Any(y => y.Id == x.UniversalProvincialEffectId))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidAntilegacyEffectIds()
        {
            var source = this.GetRepository<ITechnologyLevel>();
            var invalid = source.DataSet
                .Where(x => x.AntilegacyProvincialEffectId.HasValue)
                .Where(x => x.AntilegacyProvincialEffectId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentAntilegacyEffects()
        {
            var source = this.GetRepository<ITechnologyLevel>();
            var effectSource = this.GetRepository<IProvincialEffect>();
            var invalid = source.DataSet
                .Where(x => x.AntilegacyProvincialEffectId.HasValue)
                .Where(x => !effectSource.DataSet.Any(y => y.Id == x.AntilegacyProvincialEffectId))
                .ToList();

            Assert.Zero(invalid.Count);
        }
    }
}