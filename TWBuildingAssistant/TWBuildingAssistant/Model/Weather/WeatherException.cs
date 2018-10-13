namespace TWBuildingAssistant.Model.Weather
{
    using System;

    public class WeatherException : Exception
    {
        public WeatherException()
        : base("Failure concerning weathers.")
        {
        }
        
        public WeatherException(string message)
        : base(message)
        {
        }
        
        public WeatherException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}