namespace TWBuildingAssistant.Model.ClimateAndWeather
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public class Climate
    {
        public string Name { get; }

        public Effects.IProvincionalEffect Effect => this.effects[_currentWeatherIndex];

        public Climate(XElement element, WeatherManager weatherManager)
        {
            this.Name = (string)element.Attribute("n");
            var tuples = from XElement weatherElement in element.Elements()
                         select ((int)weatherElement.Attribute("po"), (int)weatherElement.Attribute("f"),
                                    (int)weatherElement.Attribute("s"));
            this.effects = (from ValueTuple<int, int, int> item in tuples
                            select new Effects.ProvincionalEffect()
                                   {
                                   PublicOrder = item.Item1,
                                   RegularFood = item.Item2,
                                   ProvincionalSanitation = item.Item3
                                   } as Effects.IProvincionalEffect).ToArray();

            // Aktualizacja przez event.
            weatherManager.WorstCaseWeatherChanged += (WeatherManager sender, EventArgs e) =>
                {
                    _currentWeatherIndex = (int)sender.WorstCaseWeather;
                };
        }

        private readonly Effects.IProvincionalEffect[] effects;

        private int _currentWeatherIndex = 2;
    }
}
