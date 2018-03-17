using System;
using System.Xml;
using System.Globalization;
namespace ClimateAndWeather
{
	/// <summary>
	/// Obiekt tej klasy jest opisem jednego klimatu.
	/// </summary>
	public class Climate
	{
		private int[] _publicOrder;
		private int[] _food;
		internal Climate(XmlNode node)
		{
			if (node == null)
				throw new ArgumentNullException("node");
			if (node.Name != "climate")
				throw new ArgumentException("Given node is not climate node.", "node");
			try
			{
				Name = node.Attributes.GetNamedItem("n").InnerText;
			}
			catch (Exception exception)
			{
				throw new FormatException("XML does not contain 'n' attribute (name).", exception);
			}
			//
			XmlNodeList weatherNodes = node.ChildNodes;
			_publicOrder = new int[ClimateManager.Singleton.WeatherTypesCount];
			_food = new int[ClimateManager.Singleton.WeatherTypesCount];
			if (weatherNodes.Count != ClimateManager.Singleton.WeatherTypesCount)
				throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Invalid ammount of child nodes for climate {0} (is {1}, should be {2}).", Name, weatherNodes.Count, ClimateManager.Singleton.WeatherTypesCount));
			for(int whichWeather = 0; whichWeather < ClimateManager.Singleton.WeatherTypesCount; ++whichWeather)
			{
				if (weatherNodes[whichWeather].Name != "for_weather")
					throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Expected node for_weather but instead got {0} (number {1}).", weatherNodes[whichWeather].Name, whichWeather));
				string name;
				try
				{
					name = weatherNodes[whichWeather].Attributes.GetNamedItem("n").InnerText;
				}
				catch(Exception exception)
				{
					throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Could not read 'n' attribute in for_weather node number {0}.", whichWeather), exception);
				}
				if (name != ClimateManager.Singleton.GetWeatherNameByIndex(whichWeather))
					throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Name attribute in for_weather is not matching corresponding weather name (number {0}).", whichWeather));
				try
				{
					_food[whichWeather] = Convert.ToInt32(weatherNodes[whichWeather].Attributes.GetNamedItem("f").InnerText, CultureInfo.InvariantCulture);
				_publicOrder[whichWeather] = Convert.ToInt32(weatherNodes[whichWeather].Attributes.GetNamedItem("po").InnerText, CultureInfo.InvariantCulture);
				}
				catch (Exception exception)
				{
					throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Could not create description of weather {1} for climate {0}.", Name, name), exception);
				}
			}
		}
		/// <summary>
		/// Nazwa klimatu.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// Zwraca wpływ tego klimatu na porządek publiczny przy przyjętej najgorszej pogodzie.
		/// </summary>
		public int PublicOrder
		{
			get
			{
				int index = ClimateManager.Singleton.GetWeatherIndex(ClimateManager.Singleton.WorstCaseWeather);
				return _publicOrder[index];
			}
		}
		/// <summary>
		/// Zwraca wpływ tego klimatu na żywność przy przyjętej najgorszej pogodzie.
		/// </summary>
		public int Food
		{
			get
			{
				int index = ClimateManager.Singleton.GetWeatherIndex(ClimateManager.Singleton.WorstCaseWeather);
				return _food[index];
			}
		}
	}
}
