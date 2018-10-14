namespace Tests.Model.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model;
    using TWBuildingAssistant.Model.Resources;

    using Unity;

    [TestFixture]
    public class ResourcesManagerTests
    {
        private const int ResourcesCount = 3;

        private List<Mock<IResource>> resources;

        private Mock<ISource> source;

        private IUnityContainer resolver;

        [SetUp]
        public void SetUp()
        {
            this.resources = new List<Mock<IResource>>();
            for (var i = 1; i <= ResourcesCount; ++i)
            {
                var next = new Mock<IResource>();
                string message;
                next.Setup(x => x.Id).Returns(i);
                next.Setup(x => x.Name).Returns(i.ToString());
                next.Setup(x => x.Validate(out message)).Returns(true);
                this.resources.Add(next);
            }

            this.source = new Mock<ISource>();
            this.source.Setup(x => x.Resources).Returns(from Mock<IResource> item in this.resources select item.Object);

            this.resolver = new UnityContainer();
            this.resolver.RegisterInstance<ISource>(this.source.Object);
        }

        [Test]
        public void CorrectCreation()
        {
            ResourcesManager manager = null;
            Assert.DoesNotThrow(() => manager = new ResourcesManager(this.resolver), "Exception was thrown despite the data being correct.");
            Assert.DoesNotThrow(() => manager.Find(ResourcesCount - 1), "The resources count is not matching");
        }

        [Test]
        public void CorrectParse()
        {
            var manager = new ResourcesManager(this.resolver);
            for (var i = 1; i <= ResourcesCount; ++i)
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
            var manager = new ResourcesManager(this.resolver);
            Assert.Throws<ArgumentNullException>(() => manager.Parse(null), "Attempt to parse null did not ended with exception.");
        }

        [Test]
        public void NonExistentParse()
        {
            var manager = new ResourcesManager(this.resolver);
            Assert.Throws<ResourcesException>(() => manager.Parse("0"), "Attempt to parse non-existent resource name did not ended with exception.");
        }

        [Test]
        public void InvalidResource()
        {
            string message;
            this.resources[ResourcesCount - 1].Setup(x => x.Validate(out message)).Returns(false);
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ResourcesException>(() => new ResourcesManager(this.resolver), $"{nameof(ResourcesManager)}'s constructor did not react to an invalid resource.");
        }
    }
}