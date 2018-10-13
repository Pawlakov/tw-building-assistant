﻿namespace TWBuildingAssistant.Model.Climate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TWBuildingAssistant.Model.Religions;
    using TWBuildingAssistant.Model.Weather;

    public partial class ClimateManager
    {
        private readonly IEnumerable<IClimate> climates;

        public ClimateManager(IClimateSource source, IParser<IWeather> weatherParser, IConsideredWeatherTracker consideredWeatherTracker, IParser<IReligion> religionParser)
        {
            this.climates = source.Climates.ToArray();

            foreach (var climate in this.climates)
            {
                climate.WeatherParser = weatherParser;
                foreach (var weatherEffect in climate.WeatherEffects)
                {
                    foreach (var influence in weatherEffect.Effect.Influences)
                    {
                        influence.ReligionParser = religionParser;
                    }
                }
                if (!climate.Validate(out string message))
                {
                    throw new ClimateException($"One of climates is not valid ({message}).");
                }
            }

            consideredWeatherTracker.ConsideredWeatherChanged += this.OnConsideredWeatherChanged;
        }

        private void OnConsideredWeatherChanged(object sender, ConsideredWeatherChangedArgs args)
        {
            foreach (var climate in this.climates)
            {
                climate.RefreshEffect();
            }
        }
    }

    public partial class ClimateManager : IParser<IClimate>
    {
        public IClimate Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.Equals("State", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var result = this.climates.FirstOrDefault(element => input.Equals(element.Name, StringComparison.OrdinalIgnoreCase));
            if (result == null)
            {
                throw new ClimateException("No matching climate found.");
            }

            return result;
        }

        public IClimate Find(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var result = this.climates.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                throw new ClimateException("No matching climate found.");
            }

            return result;
        }
    }
}
