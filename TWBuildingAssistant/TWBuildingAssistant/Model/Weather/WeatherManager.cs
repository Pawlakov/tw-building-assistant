namespace TWBuildingAssistant.Model.Weather
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class WeatherManager
    {
        private readonly IEnumerable<IWeather> weathers;

        public WeatherManager(IWeatherSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            try
            {
                this.weathers = source.Weathers.ToArray();
            }
            catch (Exception e)
            {
                throw new WeatherException("Unable to get weathers from the source. See inner exception for details.", e);
            }
            
            var message = string.Empty;
            if (this.weathers.Any(x => !x.Validate(out message)))
            {
                throw new WeatherException($"One of weathers is not valid ({message}).");
            }

            foreach (var weather in this.weathers)
            {
                weather.ConsideredWeatherTracker = this;
            }
        }

        public IEnumerable<KeyValuePair<int, string>> AllWeathersNames
        {
            get
            {
                var result = this.weathers.Select(x => new KeyValuePair<int, string>(x.Id, x.Name));
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
            var newConsideredWeathers = whichWeathers.Select(x => this.weathers.FirstOrDefault(y => y.Id == x)).ToArray();
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

    public partial class WeatherManager : IParser<IWeather>
    {
        public IWeather Parse(string input)
        {
            if (input == null)
            {
                return null;
            }

            var result = this.weathers.FirstOrDefault(element => input.Equals(element.Name, StringComparison.OrdinalIgnoreCase));
            if (result == null)
            {
                throw new WeatherException("No matching weather found.");
            }

            return result;
        }

        public IWeather Find(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var result = this.weathers.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                throw new WeatherException("No matching weather found.");
            }

            return result;
        }
    }
}
