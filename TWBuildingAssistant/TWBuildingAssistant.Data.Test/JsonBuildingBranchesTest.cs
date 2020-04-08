namespace TWBuildingAssistant.Data.Test
{
    using System.Linq;
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class JsonBuildingBranchesTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new BuildingBranchRepository();
            });
        }

        [Test]
        public void Any()
        {
            BuildingBranchRepository source = null;
            try
            {
                source = new BuildingBranchRepository();
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
            var source = this.GetRepository<IBuildingBranch>();
            var invalid = source.DataSet
                .Where(x => x.Id < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NullNames()
        {
            var source = this.GetRepository<IBuildingBranch>();
            var invalid = source.DataSet
                .Where(x => x.Name == null)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void EmptyNames()
        {
            var source = this.GetRepository<IBuildingBranch>();
            var invalid = source.DataSet
                .Where(x => x.Name == string.Empty)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void DuplicateIds()
        {
            var source = this.GetRepository<IBuildingBranch>();
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
            var source = this.GetRepository<IBuildingBranch>();
            var invalid = source.DataSet
                .GroupBy(x => x.Name)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidRootIds()
        {
            var source = this.GetRepository<IBuildingBranch>();
            var invalid = source.DataSet
                .Where(x => x.RootBuildingLevelId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentRoots()
        {
            var source = this.GetRepository<IBuildingBranch>();
            var levelSource = this.GetRepository<IBuildingLevel>();
            var invalid = source.DataSet
                .Where(x => !levelSource.DataSet.Any(y => y.Id == x.RootBuildingLevelId))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void DuplicateRoots()
        {
            var source = this.GetRepository<IBuildingBranch>();
            var invalid = source.DataSet
                .GroupBy(x => x.RootBuildingLevelId)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidReligionIds()
        {
            var source = this.GetRepository<IBuildingBranch>();
            var invalid = source.DataSet
                .Where(x => x.ReligionId.HasValue)
                .Where(x => x.ReligionId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentReligionIds()
        {
            var source = this.GetRepository<IBuildingBranch>();
            var religionSource = this.GetRepository<IReligion>();
            var invalid = source.DataSet
                .Where(x => x.ReligionId.HasValue)
                .Where(x => !religionSource.DataSet.Any(y => y.Id == x.ReligionId))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidResourceIds()
        {
            var source = this.GetRepository<IBuildingBranch>();
            var invalid = source.DataSet
                .Where(x => x.ResourceId.HasValue)
                .Where(x => x.ResourceId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentResourceIds()
        {
            var source = this.GetRepository<IBuildingBranch>();
            var resourceSource = this.GetRepository<IResource>();
            var invalid = source.DataSet
                .Where(x => x.ResourceId.HasValue)
                .Where(x => !resourceSource.DataSet.Any(y => y.Id == x.ResourceId))
                .ToList();

            Assert.Zero(invalid.Count);
        }
    }
}