// ReSharper disable ObjectCreationAsStatement
namespace Tests.Model.Weather
{
    using System;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model;
    using TWBuildingAssistant.Model.Weather;

    using Unity;

    [TestFixture]
    public class WeatherManagerTests
    {
        private ISource validSource;

        private ISource invalidSource;

        private ISource brokenSource;

        private ISource threeWeathersSource;

        private IUnityContainer resolver;

        [OneTimeSetUp]
        public void SetUp()
        {
            string useless;
            var correctWeather = new Mock<IWeather>();
            var incorrectWeather = new Mock<IWeather>();
            correctWeather.Setup(x => x.Validate(out useless)).Returns(true);
            incorrectWeather.Setup(x => x.Validate(out useless)).Returns(false);
            var validWeathers = new[] { correctWeather.Object, correctWeather.Object };
            var invalidWeathers = new[] { correctWeather.Object, incorrectWeather.Object };

            var valid = new Mock<ISource>();
            var invalid = new Mock<ISource>();
            var broken = new Mock<ISource>();
            var three = new Mock<ISource>();
            valid.Setup(x => x.Weathers).Returns(validWeathers);
            invalid.Setup(x => x.Weathers).Returns(invalidWeathers);
            broken.Setup(x => x.Weathers).Throws(new Exception("Literally anything."));
            three.Setup(x => x.Weathers).Returns(new[]
                                                     {
                                                         new Weather { Id = 1, Name = "1" },
                                                         new Weather { Id = 2, Name = "2" },
                                                         new Weather { Id = 3, Name = "3" }
                                                     });
            this.validSource = valid.Object;
            this.invalidSource = invalid.Object;
            this.brokenSource = broken.Object;
            this.threeWeathersSource = three.Object;

            this.resolver = new UnityContainer();
        }

        [Test]
        public void ValidConstructionTest()
        {
            this.resolver.RegisterInstance(this.validSource);
            Assert.DoesNotThrow(
                () => new WeatherManager(this.resolver),
                $"Construction of a {nameof(WeatherManager)} object using a valid {nameof(ISource)} failed.");
        }

        [Test]
        public void InvalidConstructionTest()
        {
            this.resolver.RegisterInstance(this.invalidSource);
            Assert.Throws<WeatherException>(
                () => new WeatherManager(this.resolver),
                $"Construction of a {nameof(WeatherManager)} object using an invalid {nameof(ISource)} didn't fail.");
        }

        [Test]
        public void BrokenConstructionTest()
        {
            this.resolver.RegisterInstance(this.brokenSource);
            Assert.Throws<WeatherException>(
                () => new WeatherManager(this.resolver),
                $"Construction of a {nameof(WeatherManager)} object using a broken {nameof(ISource)} didn't fail.");
        }

        [Test]
        public void NullConstructionTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new WeatherManager(null),
                $"Construction of a {nameof(WeatherManager)} object using no {nameof(ISource)} didn't fail.");
        }

        [Test]
        public void AllWeatherNamesTest()
        {
            this.resolver.RegisterInstance(this.threeWeathersSource);
            var manager = new WeatherManager(this.resolver);
            var names = manager.AllWeathersNames.ToArray();
            Assert.AreEqual(3, names.Length, $"The {nameof(WeatherManager.AllWeathersNames)} property returned wrong amount of elements.");
            for (var index = 1; index <= 3; index++)
            {
                var pair = names.FirstOrDefault(x => x.Key == index);
                Assert.IsNotNull(pair, $"The {nameof(WeatherManager.AllWeathersNames)} property didn't return an element with an expected id.");
                Assert.AreEqual(index.ToString(), pair.Value, $"The {nameof(WeatherManager.AllWeathersNames)} property didn't return an element with an expected name.");
            }
        }

        [Test]
        public void ParseValidTest()
        {
            this.resolver.RegisterInstance(this.threeWeathersSource);
            var manager = new WeatherManager(this.resolver);
            for (var index = 1; index <= 3; index++)
            {
                IWeather parsed = null;
                Assert.DoesNotThrow(() => parsed = manager.Parse(index.ToString()), $"The {nameof(WeatherManager.Parse)} method failed.");
                Assert.IsNotNull(parsed, $"The {nameof(WeatherManager.Parse)} method returned null.");
            }
        }

        [Test]
        public void ParseInvalidTest()
        {
            this.resolver.RegisterInstance(this.threeWeathersSource);
            var manager = new WeatherManager(this.resolver);
            Assert.Throws<WeatherException>(() => manager.Parse("0"), $"The {nameof(WeatherManager.Parse)} method didn't throw.");
        }

        [Test]
        public void ParseNullTest()
        {
            this.resolver.RegisterInstance(this.threeWeathersSource);
            var manager = new WeatherManager(this.resolver);
            IWeather parsed = null;
            Assert.DoesNotThrow(() => parsed = manager.Parse(null), $"The {nameof(WeatherManager.Parse)} method failed.");
            Assert.IsNull(parsed, $"The {nameof(WeatherManager.Parse)} method didn't return null.");
        }

        [Test]
        public void FindValidTest()
        {
            this.resolver.RegisterInstance(this.threeWeathersSource);
            var manager = new WeatherManager(this.resolver);
            for (var index = 1; index <= 3; index++)
            {
                IWeather found = null;
                Assert.DoesNotThrow(() => found = manager.Find(index), $"The {nameof(WeatherManager.Find)} method failed.");
                Assert.IsNotNull(found, $"The {nameof(WeatherManager.Find)} method returned null.");
            }
        }

        [Test]
        public void FindInvalidTest()
        {
            this.resolver.RegisterInstance(this.threeWeathersSource);
            var manager = new WeatherManager(this.resolver);
            Assert.Throws<WeatherException>(() => manager.Find(0), $"The {nameof(WeatherManager.Find)} method didn't throw.");
        }

        [Test]
        public void FindNullTest()
        {
            this.resolver.RegisterInstance(this.threeWeathersSource);
            var manager = new WeatherManager(this.resolver);
            IWeather found = null;
            Assert.DoesNotThrow(() => found = manager.Find(null), $"The {nameof(WeatherManager.Find)} method failed.");
            Assert.IsNull(found, $"The {nameof(WeatherManager.Find)} method didn't return null.");
        }

        [Test]
        public void ConsideredWeatherDefaultTest()
        {
            this.resolver.RegisterInstance(this.threeWeathersSource);
            var manager = new WeatherManager(this.resolver);
            var count = 0;
            Assert.DoesNotThrow(() => count = manager.ConsideredWeathers.Count(), "Checking considered weathers failed.");
            Assert.AreEqual(0, count, "There are some selected weathers by default.");
        }

        [Test]
        public void ConsideredWeatherValidChangeTest()
        {
            this.resolver.RegisterInstance(this.threeWeathersSource);
            var manager = new WeatherManager(this.resolver);
            Assert.DoesNotThrow(() => manager.ChangeConsideredWeather(new[] { 1, 3 }), $"The {nameof(WeatherManager.ChangeConsideredWeather)} method failed.");
            IWeather[] considered = null;
            Assert.DoesNotThrow(() => considered = manager.ConsideredWeathers.ToArray(), "Checking considered weathers failed.");
            Assert.AreEqual(2, considered.Length, "Wrong considered weathers count.");
            Assert.AreEqual(1, considered.Count(x => x.Id == 1), "Missing considered weather.");
            Assert.AreEqual(1, considered.Count(x => x.Id == 3), "Missing considered weather.");
        }

        [Test]
        public void ConsideredWeatherNullChangeTest()
        {
            this.resolver.RegisterInstance(this.threeWeathersSource);
            var manager = new WeatherManager(this.resolver);
            Assert.Throws<ArgumentNullException>(() => manager.ChangeConsideredWeather(null), $"The {nameof(WeatherManager.ChangeConsideredWeather)} didn't fail.");
        }

        [Test]
        public void ConsideredWeatherInvalidChangeTest()
        {
            this.resolver.RegisterInstance(this.threeWeathersSource);
            var manager = new WeatherManager(this.resolver);
            Assert.Throws<ArgumentOutOfRangeException>(() => manager.ChangeConsideredWeather(new[] { 1, 4 }), $"The {nameof(WeatherManager.ChangeConsideredWeather)} didn't fail.");
        }

        [Test]
        public void ConsideredWeatherChangeEventTest()
        {
            bool eventRaised = false;
            IConsideredWeatherTracker tracker = null;
            IWeather[] newSet = null;
            this.resolver.RegisterInstance(this.threeWeathersSource);
            var manager = new WeatherManager(this.resolver);
            manager.ConsideredWeatherChanged += (o, args) =>
                {
                    eventRaised = true;
                    tracker = args.Tracker;
                    newSet = args.NewConsideredWeathers.ToArray();
                };
            manager.ChangeConsideredWeather(new[] { 1, 3 });

            Assert.True(eventRaised, $"The {nameof(WeatherManager.ConsideredWeatherChanged)} event wasn't raised.");
            Assert.AreEqual(manager, tracker, "Wrong tracker in events arguments.");
            Assert.IsNotNull(newSet, "Missing new set of weathers in events arguments.");
            Assert.AreEqual(2, newSet.Count(), "Wrong considered weathers count.");
            Assert.AreEqual(1, newSet.Count(x => x.Id == 1), "Missing considered weather.");
            Assert.AreEqual(1, newSet.Count(x => x.Id == 3), "Missing considered weather.");
        }
    }
}