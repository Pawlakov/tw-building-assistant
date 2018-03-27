using System;
using System.Xml.Linq;
namespace ClimateAndWeather
{
	internal class Weather
	{
		// Interfejs wewnętrzny:
		//
		internal Weather(XElement element)
		{
			Name = (string)element.Attribute("n");
		}
		public override string ToString()
		{
			return Name;
		}
		//
		// Stan wewnętrzny:
		//
		private string Name { get; }
	}
}
