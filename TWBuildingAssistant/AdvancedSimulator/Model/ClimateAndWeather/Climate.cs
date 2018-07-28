namespace TWBuildingAssistant.Model.ClimateAndWeather
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public class Climate
    {
        public string Name { get; }

        public int Sanitation
        {
            get
            {
                return _sanitation[_currentWeatherIndex];
            }
        }

        public int PublicOrder
        {
            get
            {
                return _publicOrder[_currentWeatherIndex];
            }
        }

        public int Food
        {
            get
            {
                return _food[_currentWeatherIndex];
            }
        }

        public Climate(XElement element, WeatherManager weatherManager)
        {
            Name = (string)element.Attribute("n");
            _publicOrder = (from XElement weatherElement in element.Elements() select (int)weatherElement.Attribute("po")).ToArray();
            _food = (from XElement weatherElement in element.Elements() select (int)weatherElement.Attribute("f")).ToArray();
            _sanitation = (from XElement weatherElement in element.Elements() select (int)weatherElement.Attribute("s")).ToArray();
            // Aktualizacja przez event.
            weatherManager.WorstCaseWeatherChanged += (WeatherManager sender, EventArgs e) => { _currentWeatherIndex = (int)sender.WorstCaseWeather; };
        }

        private readonly int[] _publicOrder;

        private readonly int[] _food;

        private readonly int[] _sanitation;

        private int _currentWeatherIndex = 2;
    }
}
