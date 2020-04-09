namespace TWBuildingAssistant.Data.Test
{
    using System.Linq;
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class JsonRegionalEffectsTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new JsonRepository<IRegionalEffect>();
            });
        }

        [Test]
        public void Any()
        {
            JsonRepository<IRegionalEffect> source = null;
            try
            {
                source = new JsonRepository<IRegionalEffect>();
            }
            catch
            {
                Assert.Inconclusive();
            }

            Assert.NotZero(source.DataSet.Count());
        }

        [Test]
        public void InvalidIds()
        {
            var source = this.GetRepository<IRegionalEffect>();
            var invalidIds = source.DataSet
                .Where(x => x.Id < 1)
                .ToList();

            Assert.Zero(invalidIds.Count);
        }

        [Test]
        public void DuplicateIds()
        {
            var source = this.GetRepository<IRegionalEffect>();
            var duplicateIds = source.DataSet
                .GroupBy(x => x.Id)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(duplicateIds.Count);
        }
    }
}