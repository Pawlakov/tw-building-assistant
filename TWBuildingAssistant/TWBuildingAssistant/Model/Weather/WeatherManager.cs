namespace TWBuildingAssistant.Model.Weather
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Unity;

    public partial class WeatherManager : Parser<IWeather>
    {
        public WeatherManager(IUnityContainer resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            try
            {
                this.Content = resolver.Resolve<ISource>().Weathers.ToArray();
            }
            catch (Exception e)
            {
                throw new WeatherException("Unable to get weathers from the source. See inner exception for details.", e);
            }
            
            var message = string.Empty;
            if (this.Content.Any(x => !x.Validate(out message)))
            {
                throw new WeatherException($"One of weathers is not valid ({message}).");
            }

            foreach (var weather in this.Content)
            {
                weather.ConsideredWeatherTracker = this;
            }
        }

        public IEnumerable<KeyValuePair<int, string>> AllWeathersNames
        {
            get
            {
                var result = this.Content.Select(x => new KeyValuePair<int, string>(x.Id, x.Name));
                return result;
            }
        }
    }

    public partial class WeatherManager : IConsideredWeatherTracker
    {
        public event EventHandler<ConsideredWeatherChangedArgs> ConsideredWeatherChanged;

        public IEnumerable<IWeather> ConsideredWeathers { get; private set; } = new IWeather[0];

        public void ChangeConsideredWeather(IEnumerable<int> whichWeathers)
        {
            var newConsideredWeathers = whichWeathers.Select(x => this.Content.FirstOrDefault(y => y.Id == x)).ToArray();
            if (newConsideredWeathers.Any(x => x == null))
            {
                throw new ArgumentOutOfRangeException(nameof(whichWeathers), whichWeathers, $"There is no weather corresponding to one of given ids.");
            }

            this.ConsideredWeathers = newConsideredWeathers;
            this.OnConsideredWeatherChanged(new ConsideredWeatherChangedArgs(this, this.ConsideredWeathers));
        }

        private void OnConsideredWeatherChanged(ConsideredWeatherChangedArgs e)
        {
            this.ConsideredWeatherChanged?.Invoke(this, e);
        }
    }
}
