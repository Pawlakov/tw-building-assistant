namespace TWBuildingAssistant.Data.Test
{
    using System.Linq;
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class JsonBuildingBranchUsesTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new JsonRepository<IBuildingBranchUse>();
            });
        }

        [Test]
        public void Any()
        {
            JsonRepository<IBuildingBranchUse> source = null;
            try
            {
                source = new JsonRepository<IBuildingBranchUse>();
            }
            catch
            {
                Assert.Inconclusive();
            }

            Assert.NotZero(source.DataSet.Count());
        }

        [Test]
        public void InvalidFactionIds()
        {
            var source = this.GetRepository<IBuildingBranchUse>();
            var invalid = source.DataSet
                .Where(x => x.FactionId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentFactions()
        {
            var source = this.GetRepository<IBuildingBranchUse>();
            var factionSource = this.GetRepository<IFaction>();
            var invalid = source.DataSet
                .Where(x => !factionSource.DataSet.Any(y => y.Id == x.FactionId))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidBranchIds()
        {
            var source = this.GetRepository<IBuildingBranchUse>();
            var invalid = source.DataSet
                .Where(x => x.BuildingBranchId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentBranches()
        {
            var source = this.GetRepository<IBuildingBranchUse>();
            var branchSource = this.GetRepository<IBuildingBranch>();
            var invalid = source.DataSet
                .Where(x => !branchSource.DataSet.Any(y => y.Id == x.BuildingBranchId))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void DuplicateUses()
        {
            var source = this.GetRepository<IBuildingBranchUse>();
            var invalid = source.DataSet
                .GroupBy(x => new { x.FactionId, x.BuildingBranchId })
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(invalid.Count);
        }
    }
}