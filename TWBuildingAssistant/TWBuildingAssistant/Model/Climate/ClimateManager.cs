namespace TWBuildingAssistant.Model.Climate
{
    using System.Collections.Generic;
    using System.Linq;

    using TWBuildingAssistant.Model.Religions;
    using TWBuildingAssistant.Model.Weather;

    using Unity;

    public class ClimateManager : Parser<IClimate>
    {
        public ClimateManager(IUnityContainer resolver)
        {
            this.Content = resolver.Resolve<ISource>().Climates.ToArray();

            foreach (var climate in this.Content)
            {
                climate.WeatherParser = resolver.Resolve<Parser<IWeather>>();
                foreach (var weatherEffect in climate.WeatherEffects)
                {
                    foreach (var influence in weatherEffect.Effect.Influences)
                    {
                        influence.SetReligionParser(resolver.Resolve<Parser<IReligion>>());
                    }
                }
                if (!climate.Validate(out string message))
                {
                    throw new ClimateException($"One of climates is not valid ({message}).");
                }
            }

            resolver.Resolve<IConsideredWeatherTracker>().ConsideredWeatherChanged += this.OnConsideredWeatherChanged;
        }

        private void OnConsideredWeatherChanged(object sender, ConsideredWeatherChangedArgs args)
        {
            foreach (var climate in this.Content)
            {
                climate.RefreshEffect();
            }
        }
    }
}
