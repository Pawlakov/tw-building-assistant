using System;
using System.Xml;
namespace ClimateAndWeather
{
	internal class Weather
	{
		// Interfejs wewnętrzny:
		//
		internal Weather(XmlNode node)
		{
			if (node.Name != "weather")
				throw new ArgumentException("Given XML node is not weather node.");
			XmlNode temporary = node.Attributes.GetNamedItem("n");
			if(temporary == null)
				throw new FormatException("Weather XML node does not contain name attribute.");
			Name = temporary.InnerText;
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
