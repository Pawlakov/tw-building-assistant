namespace Tests.Model.Climate
{
    using System;

    using Moq;

    using NUnit.Framework;

    using TWBuildingAssistant.Model;
    using TWBuildingAssistant.Model.Climate;
    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Weather;

    [TestFixture]
    public class WeatherEffectTests
    {
        private Mock<Parser<IWeather>> parser;

        private Mock<IWeather> weather;

        private Mock<IProvincialEffect> validEffect;

        private Mock<IProvincialEffect> invalidEffect;

        [OneTimeSetUp]
        public void SetUp()
        {
            string message;

            this.weather = new Mock<IWeather>();
            this.weather.Setup(x => x.Name).Returns("1");

            this.parser = new Mock<Parser<IWeather>>();
            this.parser.Setup(x => x.Find(It.IsAny<int?>())).Returns<int?>(
                x =>
                {
                    return x == 1 ? this.weather.Object : null;
                });

            this.validEffect = new Mock<IProvincialEffect>();
            this.validEffect.Setup(x => x.Validate(out message)).Returns(true);

            this.invalidEffect = new Mock<IProvincialEffect>();
            this.invalidEffect.Setup(x => x.Validate(out message)).Returns(false);
        }

        [Test]
        public void ValidateValidTest()
        {
            var weatherEffect = new WeatherEffect()
            {
                WeatherId = 1,
                Effect = this.validEffect.Object,
            };
            weatherEffect.SetWeatherParser(this.parser.Object);
            Assert.AreEqual(true, weatherEffect.Validate(out _), $"The method returned false.");
        }
        
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(null, true)]
        public void ValidateInvalidTest(bool? useValidEffect, bool useParser)
        {
            var weatherEffect = new WeatherEffect
            {
                WeatherId = 1,
                Effect = null
            };
            if (useValidEffect.HasValue)
            {
                weatherEffect.Effect = useValidEffect.Value ? this.validEffect.Object : this.invalidEffect.Object;
            }

            if (useParser)
            {
                weatherEffect.SetWeatherParser(this.parser.Object);
            }

            Assert.AreEqual(false, weatherEffect.Validate(out _), $"The method returned true.");
        }

        [Test]
        public void GetWeatherConcreteTest()
        {
            var weatherEffect = new WeatherEffect
            {
                WeatherId = 1,
                Effect = this.validEffect.Object
            };
            weatherEffect.SetWeatherParser(this.parser.Object);
            IWeather parsed = null;
            Assert.DoesNotThrow(() => parsed = weatherEffect.GetWeather(), $"The method failed.");
            Assert.AreEqual(this.weather.Object, parsed, $"Received wrong {nameof(IWeather)} object.");
        }

        [Test]
        public void GetWeatherNullParserTest()
        {
            var weatherEffect = new WeatherEffect
            {
                WeatherId = 1,
                Effect = this.validEffect.Object
            };
            Assert.Throws<ArgumentNullException>(() => weatherEffect.SetWeatherParser(null), $"The method didn't throw {nameof(ArgumentNullException)}.");
        }

        [Test]
        public void GetWeatherNotSetParserTest()
        {
            var weatherEffect = new WeatherEffect
            {
                WeatherId = 1,
                Effect = this.validEffect.Object
            };
            Assert.Throws<ClimateException>(() => weatherEffect.GetWeather(), $"The method didn't throw {nameof(ClimateException)}.");
        }

        [Test]
        public void GetWeatherNonexistentTest()
        {
            var weatherEffect = new WeatherEffect
            {
                WeatherId = 2,
                Effect = this.validEffect.Object,
            };
            Assert.DoesNotThrow(() => weatherEffect.SetWeatherParser(this.parser.Object), $"The method failed.");
            Assert.Throws<ClimateException>(() => weatherEffect.GetWeather(), $"The method didn't throw {nameof(ClimateException)}.");
        }
    }
}