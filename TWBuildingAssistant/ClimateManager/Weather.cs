using System;
using System.Xml;
namespace ClimateAndWeather
{
	internal class Weather
	{
		internal Weather(XmlNode node)
		{
			if (node == null)
				throw new ArgumentNullException("node");
			if (node.Name != "weather")
				throw new ArgumentException("Given node is not weather node.", "node");
			try
			{
				Name = node.Attributes.GetNamedItem("n").InnerText;
			}
			catch (Exception exception)
			{
				throw new FormatException("XML does not contain 'n' attribute (name).", exception);
			}
		}
		public string Name { get; }
	}
}
