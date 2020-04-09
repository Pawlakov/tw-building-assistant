namespace TWBuildingAssistant.Data.Test
{
    using NUnit.Framework;
    using System.Linq;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class JsonBuildingLevelLocksTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new JsonRepository<IBuildingLevelLock>();
            });
        }

        [Test]
        public void Any()
        {
            JsonRepository<IBuildingLevelLock> source = null;
            try
            {
                source = new JsonRepository<IBuildingLevelLock>();
            }
            catch
            {
                Assert.Inconclusive();
            }

            Assert.NotZero(source.DataSet.Count());
        }
    }
}