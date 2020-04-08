namespace TWBuildingAssistant.Data.Test
{
    using System.Linq;
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class JsonBuildingLevelsTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new BuildingLevelRepository();
            });
        }

        [Test]
        public void Any()
        {
            BuildingLevelRepository source = null;
            try
            {
                source = new BuildingLevelRepository();
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
            var source = this.GetRepository<IBuildingLevel>();
            var invalid = source.DataSet
                .Where(x => x.Id < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NullNames()
        {
            var source = this.GetRepository<IBuildingLevel>();
            var invalid = source.DataSet
                .Where(x => x.Name == null)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void EmptyNames()
        {
            var source = this.GetRepository<IBuildingLevel>();
            var invalid = source.DataSet
                .Where(x => x.Name == string.Empty)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void DuplicateIds()
        {
            var source = this.GetRepository<IBuildingLevel>();
            var invalid = source.DataSet
                .GroupBy(x => x.Id)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void OrphanRoots()
        {
            var source = this.GetRepository<IBuildingLevel>();
            var branchSource = this.GetRepository<IBuildingBranch>();
            var invalid = source.DataSet
                .Where(x => x.ParentBuildingLevelId == null)
                .Where(x => !branchSource.DataSet.Any(y => y.RootBuildingLevelId == x.Id))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidParentIds()
        {
            var source = this.GetRepository<IBuildingLevel>();
            var invalid = source.DataSet
                .Where(x => x.ParentBuildingLevelId != null)
                .Where(x => x.ParentBuildingLevelId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentParents()
        {
            var source = this.GetRepository<IBuildingLevel>();
            var invalid = source.DataSet
                .Where(x => x.ParentBuildingLevelId != null)
                .Where(x => !source.DataSet.Any(y => y.Id == x.ParentBuildingLevelId))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void SelfParents()
        {
            var source = this.GetRepository<IBuildingLevel>();
            var invalid = source.DataSet
                .Where(x => x.ParentBuildingLevelId != null)
                .Where(x => x.ParentBuildingLevelId == x.Id)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidEffectIds()
        {
            var source = this.GetRepository<IBuildingLevel>();
            var invalid = source.DataSet
                .Where(x => x.RegionalEffectId.HasValue)
                .Where(x => x.RegionalEffectId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentEffects()
        {
            var source = this.GetRepository<IBuildingLevel>();
            var effectSource = this.GetRepository<IRegionalEffect>();
            var invalid = source.DataSet
                .Where(x => x.RegionalEffectId.HasValue)
                .Where(x => !effectSource.DataSet.Any(y => y.Id == x.RegionalEffectId))
                .ToList();

            Assert.Zero(invalid.Count);
        }
    }
}