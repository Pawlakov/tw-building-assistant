// ReSharper disable ObjectCreationAsStatement
namespace Tests.Model.Resources
{
    using System;

    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model;
    using TWBuildingAssistant.Model.Resources;

    using Unity;

    [TestFixture]
    public class ResourceManagerTests
    {
        private Mock<ISource> validSource;

        private Mock<ISource> invalidSource;

        private Mock<ISource> brokenSource;

        private Mock<ISource> threeResourcesSource;

        private IUnityContainer resolver;

        [OneTimeSetUp]
        public void SetUp()
        {
            string useless;
            var correctResource = new Mock<IResource>();
            var incorrectResource = new Mock<IResource>();
            correctResource.Setup(x => x.Validate(out useless)).Returns(true);
            incorrectResource.Setup(x => x.Validate(out useless)).Returns(false);
            var validResources = new[] { correctResource.Object, correctResource.Object };
            var invalidResources = new[] { correctResource.Object, incorrectResource.Object };

            this.validSource = new Mock<ISource>();
            this.invalidSource = new Mock<ISource>();
            this.brokenSource = new Mock<ISource>();
            this.threeResourcesSource = new Mock<ISource>();
            this.validSource.Setup(x => x.Resources).Returns(validResources);
            this.invalidSource.Setup(x => x.Resources).Returns(invalidResources);
            this.brokenSource.Setup(x => x.Resources).Throws(new Exception("Literally anything."));
            this.threeResourcesSource.Setup(x => x.Resources).Returns(new[]
                                                     {
                                                         new Resource { Id = 1, Name = "1" },
                                                         new Resource { Id = 2, Name = "2" },
                                                         new Resource { Id = 3, Name = "3" }
                                                     });
            this.resolver = new UnityContainer();
        }

        [Test]
        public void ConstructorValidTest()
        {
            this.resolver.RegisterInstance(this.validSource.Object);
            Assert.DoesNotThrow(
                () => new ResourceManager(this.resolver),
                $"Constructor failed.");
        }

        [Test]
        public void ConstructorInvalidTest()
        {
            this.resolver.RegisterInstance(this.invalidSource.Object);
            Assert.Throws<ResourceException>(
                () => new ResourceManager(this.resolver),
                $"Constructor didn't throw {nameof(ResourceException)}.");
        }

        [Test]
        public void ConstructorBrokenTest()
        {
            this.resolver.RegisterInstance(this.brokenSource.Object);
            Assert.Throws<ResourceException>(
                () => new ResourceManager(this.resolver),
                $"Constructor didn't throw {nameof(ResourceException)}.");
        }

        [Test]
        public void ConstructorNullTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ResourceManager(null),
                $"Constructor didn't throw {nameof(ArgumentNullException)}.");
        }

        [Test]
        public void ParseValidTest()
        {
            this.resolver.RegisterInstance(this.threeResourcesSource.Object);
            var manager = new ResourceManager(this.resolver);
            for (var index = 1; index <= 3; index++)
            {
                IResource parsed = null;
                Assert.DoesNotThrow(() => parsed = manager.Parse(index.ToString()), $"The {nameof(ResourceManager.Parse)} method failed.");
                Assert.IsNotNull(parsed, $"The {nameof(ResourceManager.Parse)} method returned null.");
            }
        }

        [Test]
        public void ParseInvalidTest()
        {
            this.resolver.RegisterInstance(this.threeResourcesSource.Object);
            var manager = new ResourceManager(this.resolver);
            IResource parsed = null;
            Assert.DoesNotThrow(() => parsed = manager.Parse("0"), $"The {nameof(ResourceManager.Parse)} method failed.");
            Assert.IsNull(parsed, $"The {nameof(ResourceManager.Parse)} method didn't return null.");
        }

        [Test]
        public void ParseNullTest()
        {
            this.resolver.RegisterInstance(this.threeResourcesSource.Object);
            var manager = new ResourceManager(this.resolver);
            Assert.Throws<ArgumentNullException>(() => manager.Parse(null), $"The {nameof(ResourceManager.Parse)} method didn't throw {nameof(ArgumentNullException)}.");
        }

        [Test]
        public void FindValidTest()
        {
            this.resolver.RegisterInstance(this.threeResourcesSource.Object);
            var manager = new ResourceManager(this.resolver);
            for (var index = 1; index <= 3; index++)
            {
                IResource found = null;
                Assert.DoesNotThrow(() => found = manager.Find(index), $"The {nameof(ResourceManager.Find)} method failed.");
                Assert.IsNotNull(found, $"The {nameof(ResourceManager.Find)} method returned null.");
            }
        }

        [Test]
        public void FindInvalidTest()
        {
            this.resolver.RegisterInstance(this.threeResourcesSource.Object);
            var manager = new ResourceManager(this.resolver);
            IResource found = null;
            Assert.DoesNotThrow(() => found = manager.Find(null), $"The {nameof(ResourceManager.Find)} method failed.");
            Assert.IsNull(found, $"The {nameof(ResourceManager.Find)} method didn't return null.");
        }

        [Test]
        public void FindNullTest()
        {
            this.resolver.RegisterInstance(this.threeResourcesSource.Object);
            var manager = new ResourceManager(this.resolver);
            IResource found = null;
            Assert.DoesNotThrow(() => found = manager.Find(null), $"The {nameof(ResourceManager.Find)} method failed.");
            Assert.IsNull(found, $"The {nameof(ResourceManager.Find)} method didn't return null.");
        }
    }
}