﻿namespace TWBuildingAssistant.Data.Test
{
    using System.Linq;
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class JsonFactionsTest : JsonRepositoryTestBase
    {
        [Test]
        public void Read()
        {
            Assert.DoesNotThrow(() =>
            {
                var source = new JsonRepository<IFaction>();
            });
        }

        [Test]
        public void Any()
        {
            JsonRepository<IFaction> source = null;
            try
            {
                source = new JsonRepository<IFaction>();
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
            var source = this.GetRepository<IFaction>();
            var invalid = source.DataSet
                .Where(x => x.Id < 1)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void NullNames()
        {
            var source = this.GetRepository<IFaction>();
            var invalid = source.DataSet
                .Where(x => x.Name == null)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void EmptyNames()
        {
            var source = this.GetRepository<IFaction>();
            var invalid = source.DataSet
                .Where(x => x.Name == string.Empty)
                .ToList();

            Assert.Zero(invalid.Count);
        }

        [Test]
        public void DuplicateIds()
        {
            var source = this.GetRepository<IFaction>();
            var duplicate = source.DataSet
                .GroupBy(x => x.Id)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(duplicate.Count);
        }

        [Test]
        public void DuplicateNames()
        {
            var source = this.GetRepository<IFaction>();
            var invalid = source.DataSet
                .GroupBy(x => x.Name)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            Assert.Zero(invalid.Count);
        }
    }
}