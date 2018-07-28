namespace Tests.Model.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Resources;

    /// <summary>
    /// Test fixture containing tests of the <see cref="ResourcesManager"/> class.
    /// </summary>
    [TestFixture]
    public class ResourcesManagerTests
    {
        /// <summary>
        /// Number of resource mocks used during tests.
        /// </summary>
        private const int ResourcesCount = 3;

        /// <summary>
        /// The list of mocks substituting resources.
        /// </summary>
        private List<Mock<IResource>> resources;

        /// <summary>
        /// The mock substituting a source of resources.
        /// </summary>
        private Mock<IResourcesSource> source;

        /// <summary>
        /// The set up before each test.
        /// </summary>
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

        /// <summary>
        /// Checks whether the creation of <see cref="ResourcesManager"/> object undergoes correctly.
        /// </summary>
        [Test]
        public void CorrectCreation()
        {
            ResourcesManager manager = null;
            Assert.DoesNotThrow(() => manager = new ResourcesManager(this.source.Object), "Exception was thrown despite the data being correct.");
            Assert.AreEqual(ResourcesCount, manager.Resources.Count(), "The resources count is not matching");
        }

        /// <summary>
        /// Checks whether the <see cref="ResourcesManager"/> object behaves correctly when parsing correct <see cref="string"/>s.
        /// </summary>
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

        /// <summary>
        /// Checks whether the <see cref="ResourcesManager"/> object behaves correctly when parsing null <see cref="string"/>.
        /// </summary>
        [Test]
        public void NullParse()
        {
            var manager = new ResourcesManager(this.source.Object);
            Assert.Throws<ArgumentNullException>(() => manager.Parse(null), "Attempt to parse null did not ended with exception.");
        }

        /// <summary>
        /// Checks whether the <see cref="ResourcesManager"/> object behaves correctly when parsing non-existent resource.
        /// </summary>
        [Test]
        public void NonExistentParse()
        {
            var manager = new ResourcesManager(this.source.Object);
            Assert.Throws<ResourcesException>(() => manager.Parse(ResourcesCount.ToString()), "Attempt to parse non-existent resource name did not ended with exception.");
        }

        /// <summary>
        /// Checks whether the <see cref="ResourcesManager"/> object behaves correctly when one of <see cref="IResource"/> objects is invalid.
        /// </summary>
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