namespace TWBuildingAssistant.Data.Test
{
    using System.Linq;
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class JsonProvincialEffectsTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new ProvincialEffectRepository();
            });
        }

        [Test]
        public void Any()
        {
            ProvincialEffectRepository source = null;
            try
            {
                source = new ProvincialEffectRepository();
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
            var source = this.GetRepository<IProvincialEffect>();
            var invalidIds = source.DataSet
                .Where(x => x.Id < 1)
                .ToList();

            Assert.Zero(invalidIds.Count);
        }

        [Test]
        public void DuplicateIds()
        {
            var source = this.GetRepository<IProvincialEffect>();
            var duplicateIds = source.DataSet
                .GroupBy(x => x.Id)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(duplicateIds.Count);
        }
    }
}