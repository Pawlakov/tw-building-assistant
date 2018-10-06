namespace TWBuildingAssistant.Model.ClimateAndWeather
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public class Climate
    {
        public string Name { get; }

        public Effects.IProvincialEffect Effect => this.effects[_currentWeatherIndex];

        public Climate(XElement element, WeatherManager weatherManager)
        {
            this.Name = (string)element.Attribute("n");
            var tuples = from XElement weatherElement in element.Elements()
                         select ((int)weatherElement.Attribute("po"), (int)weatherElement.Attribute("f"),
                                    (int)weatherElement.Attribute("s"));
            this.effects = (from ValueTuple<int, int, int> item in tuples
                            select new Effects.ProvincialEffect()
                                   {
                                   PublicOrder = item.Item1,
                                   RegularFood = item.Item2,
                                   ProvincialSanitation = item.Item3
                                   } as Effects.IProvincialEffect).ToArray();

            // Aktualizacja przez event.
            weatherManager.WorstCaseWeatherChanged += (WeatherManager sender, EventArgs e) =>
                {
                    _currentWeatherIndex = (int)sender.WorstCaseWeather;
                };
        }

        private readonly Effects.IProvincialEffect[] effects;

        private int _currentWeatherIndex = 2;
    }
}
