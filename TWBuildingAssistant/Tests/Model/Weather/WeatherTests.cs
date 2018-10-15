namespace Tests.Model.Weather
{
    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model.Weather;

    [TestFixture]
    public class WeatherTests
    {
        [Test]
        public void ValidateValidTest()
        {
            IWeather created = new Weather { Id = 1, Name = "Name" };
            Assert.AreEqual(true, created.Validate(out _), $"The {nameof(Weather.Validate)} method returned false.");
        }

        [TestCase("Name", 0)]
        [TestCase("", 1)]
        [TestCase(null, 1)]
        [TestCase("", 0)]
        public void ValidateInvalidTest(string name, int id)
        {
            IWeather created = new Weather { Id = id, Name = name };
            Assert.AreEqual(false, created.Validate(out _), $"The {nameof(Weather.Validate)} method returned true.");
        }

        [Test]
        public void ToStringTest()
        {
            const string Name = "TheName";
            IWeather created = new Weather { Id = 1, Name = Name };
            Assert.IsTrue(created.ToString().Equals(Name), $"The {nameof(Weather.ToString)} method returned unexpected value.");
        }

        [Test]
        public void IsConsideredTest()
        {
            var considered = new Weather { Id = 1, Name = "1" };
            var other = new Weather { Id = 2, Name = "2" };

            var tracker = new Mock<IConsideredWeatherTracker>();
            tracker.Setup(x => x.ConsideredWeathers).Returns(new IWeather[] { other, considered });

            var consideredSet = new ConsideredWeatherChangedArgs(tracker.Object, new IWeather[] { considered });

            considered.ConsideredWeatherTracker = tracker.Object;
            other.ConsideredWeatherTracker = tracker.Object;

            Assert.AreEqual(true, considered.IsConsidered(), $"The {nameof(Weather.IsConsidered)} method returned false instead of true.");
            Assert.AreEqual(true, other.IsConsidered(), $"The {nameof(Weather.IsConsidered)} method returned false instead of true.");
            tracker.Raise(x => x.ConsideredWeatherChanged += null, consideredSet);
            Assert.AreEqual(true, considered.IsConsidered(), $"The {nameof(Weather.IsConsidered)} method returned false instead of true.");
            Assert.AreEqual(false, other.IsConsidered(), $"The {nameof(Weather.IsConsidered)} method returned true instead of false.");
        }

        [Test]
        public void IsConsideredNoTrackerTest()
        {
            var created = new Weather { Id = 1, Name = "Name" };
            Assert.Throws<WeatherException>(() => created.IsConsidered(), $"The {nameof(Weather.IsConsidered)} property did not throw an exception.");
        }
    }
}