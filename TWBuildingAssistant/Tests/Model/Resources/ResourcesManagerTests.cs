namespace Tests.Model.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Resources;

    [TestFixture]
    public class ResourcesManagerTests
    {
        private const int ResourcesCount = 3;

        private List<Mock<IResource>> resources;

        private Mock<IResourcesSource> source;

        [SetUp]
        public void SetUp()
        {
            this.resources = new List<Mock<IResource>>();
            for (var i = 0; i < ResourcesCount; ++i)
            {
                var next = new Mock<IResource>();
                string message;
                next.Setup(x => x.Name).Returns(i.ToString());
                next.Setup(x => x.Validate(out message)).Returns(true);
                this.resources.Add(next);
            }

            this.source = new Mock<IResourcesSource>();
            this.source.Setup(x => x.GetResources()).Returns(from Mock<IResource> item in this.resources select item.Object);
        }

        [Test]
        public void CorrectCreation()
        {
            ResourcesManager manager = null;
            Assert.DoesNotThrow(() => manager = new ResourcesManager(this.source.Object), "Exception was thrown despite the data being correct.");
            Assert.DoesNotThrow(() => manager.Find(ResourcesCount - 1), "The resources count is not matching");
        }

        [Test]
        public void CorrectParse()
        {
            var manager = new ResourcesManager(this.source.Object);
            for (var i = 0; i < ResourcesCount; ++i)
            {
                IResource item = null;
                var name = i.ToString();
                Assert.DoesNotThrow(() => item = manager.Parse(name), "Attempt to parse a correct name ended with exception.");
                Assert.True(name.Equals(item.Name, StringComparison.OrdinalIgnoreCase), "The result of parsing has invalid name.");
            }
        }

        [Test]
        public void NullParse()
        {
            var manager = new ResourcesManager(this.source.Object);
            Assert.Throws<ArgumentNullException>(() => manager.Parse(null), "Attempt to parse null did not ended with exception.");
        }

        [Test]
        public void NonExistentParse()
        {
            var manager = new ResourcesManager(this.source.Object);
            Assert.Throws<ResourcesException>(() => manager.Parse(ResourcesCount.ToString()), "Attempt to parse non-existent resource name did not ended with exception.");
        }

        [Test]
        public void InvalidResource()
        {
            string message;
            this.resources[ResourcesCount - 1].Setup(x => x.Validate(out message)).Returns(false);
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ResourcesException>(() => new ResourcesManager(this.source.Object), $"{nameof(ResourcesManager)}'s constructor did not react to an invalid resource.");
        }
    }
}