namespace TWBuildingAssistant.Data.Test
{
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;

    [TestFixture]
    public class JsonBuildingLevelLocksTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new BuildingLevelLockRepository();
            });
        }

        [Test]
        public void Any()
        {
            BuildingLevelLockRepository source = null;
            try
            {
                source = new BuildingLevelLockRepository();
            }
            catch
            {
                Assert.Inconclusive();
            }

            Assert.NotZero(source.DataSet.Count);
        }
    }
}