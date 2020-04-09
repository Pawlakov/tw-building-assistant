namespace TWBuildingAssistant.Data.Test
{
    using System.Linq;
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class JsonRegionsTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new JsonRepository<IRegion>();
            });
        }

        [Test]
        public void Any()
        {
            JsonRepository<IRegion> source = null;
            try
            {
                source = new JsonRepository<IRegion>();
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
            var source = this.GetRepository<IRegion>();
            var invalid = source.DataSet
                .Where(x => x.Id < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NullNames()
        {
            var source = this.GetRepository<IRegion>();
            var invalid = source.DataSet
                .Where(x => x.Name == null)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void EmptyNames()
        {
            var source = this.GetRepository<IRegion>();
            var invalid= source.DataSet
                .Where(x => x.Name == string.Empty)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void DuplicateIds()
        {
            var source = this.GetRepository<IRegion>();
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
            var source = this.GetRepository<IRegion>();
            var invalid = source.DataSet
                .GroupBy(x => x.Name)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidSlotCountOffsets()
        {
            var source = this.GetRepository<IRegion>();
            var invalid = source.DataSet
                .Where(x => x.SlotsCountOffset < -1 || x.SlotsCountOffset > 0)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidProvinceIds()
        {
            var source = this.GetRepository<IRegion>();
            var invalid = source.DataSet
                .Where(x => x.ProvinceId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentProvinceIds()
        {
            var source = this.GetRepository<IRegion>();
            var provinceSource = this.GetRepository<IProvince>();
            var invalid = source.DataSet
                .Where(x => !provinceSource.DataSet.Any(y => y.Id == x.ProvinceId))
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void GroupedProvinceIds()
        {
            var source = this.GetRepository<IRegion>();
            var invalid = source.DataSet
                .GroupBy(x => x.ProvinceId)
                .Where(x => x.Count() != 3)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void CityProvinceIds()
        {
            var source = this.GetRepository<IRegion>();
            var invalid = source.DataSet
                .Where(x => x.IsCity)
                .GroupBy(x => x.ProvinceId)
                .Where(x => x.Count() != 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void TownProvinceIds()
        {
            var source = this.GetRepository<IRegion>();
            var invalid = source.DataSet
                .Where(x => !x.IsCity)
                .GroupBy(x => x.ProvinceId)
                .Where(x => x.Count() != 2)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void InvalidResourceIds()
        {
            var source = this.GetRepository<IRegion>();
            var invalid = source.DataSet
                .Where(x => x.ResourceId.HasValue)
                .Where(x => x.ResourceId < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NonexistentResourceIds()
        {
            var source = this.GetRepository<IRegion>();
            var resourceSource = this.GetRepository<IResource>();
            var invalid = source.DataSet
                .Where(x => x.ResourceId.HasValue)
                .Where(x => !resourceSource.DataSet.Any(y => y.Id == x.ResourceId))
                .ToList();

            Assert.Zero(invalid.Count);
        }
    }
}