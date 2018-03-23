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
		// Interfejs publiczny:
		//
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
				if (CurrentIndex < 0)
					throw new InvalidOperationException("Unknown worst case weather.");
				return _publicOrder[CurrentIndex];
			}
		}
		/// <summary>
		/// Zwraca wpływ tego klimatu na żywność przy przyjętej najgorszej pogodzie.
		/// </summary>
		public int Food
		{
			get
			{
				if (CurrentIndex < 0)
					throw new InvalidOperationException("Unknown worst case weather.");
				return _food[CurrentIndex];
			}
		}
		//
		// Interfejs wewnętrzny:
		//
		internal Climate(XmlNode node)
		{
			if (node.Name != "climate")
				throw new ArgumentException("Given node is not climate node.", "node");
			XmlNode temporary = node.Attributes.GetNamedItem("n");
			if (temporary == null)
				throw new FormatException("XML does not contain 'n' attribute (name).");
			Name = temporary.InnerText;
			//
			XmlNodeList weatherNodes = node.ChildNodes;
			_publicOrder = new int[WeatherManager.Singleton.WeatherTypesCount];
			_food = new int[WeatherManager.Singleton.WeatherTypesCount];
			if (weatherNodes.Count != WeatherManager.Singleton.WeatherTypesCount)
				throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Invalid ammount of child nodes for climate {0} (is {1}, should be {2}).", Name, weatherNodes.Count, WeatherManager.Singleton.WeatherTypesCount));
			for (int whichWeather = 0; whichWeather < WeatherManager.Singleton.WeatherTypesCount; ++whichWeather)
			{
				XmlNode currentNode = weatherNodes[whichWeather];
				XmlAttributeCollection currentAtributes = currentNode.Attributes;
				if (currentNode.Name != "for_weather")
					throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Expected node for_weather but instead got {0} (number {1}).", currentNode.Name, whichWeather));
				int weatherIndexInThisNode;
				try
				{
					weatherIndexInThisNode = Convert.ToInt32(currentAtributes.GetNamedItem("i").InnerText, CultureInfo.InvariantCulture);
				}
				catch (Exception exception)
				{
					throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Could not read index attribute in for_weather node number {0}.", whichWeather), exception);
				}
				if (weatherIndexInThisNode != whichWeather)
					throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Index attribute in for_weather is not convergent with weather (is {0} should be {1}).",weatherIndexInThisNode , whichWeather));
				try
				{
					_food[whichWeather] = Convert.ToInt32(currentAtributes.GetNamedItem("f").InnerText, CultureInfo.InvariantCulture);
					_publicOrder[whichWeather] = Convert.ToInt32(currentAtributes.GetNamedItem("po").InnerText, CultureInfo.InvariantCulture);
				}
				catch (Exception exception)
				{
					throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Could not create description of weather number {1} for climate {0}.", Name, weatherIndexInThisNode), exception);
				}
			}
			WeatherManager.Singleton.WorstCaseWeatherChanged += (WeatherManager sender, EventArgs e) => CurrentIndex = sender.WorstCaseWeatherIndex;
		}
		//
		// Stan wewnętrzny:
		//
		private readonly int[] _publicOrder;
		private readonly int[] _food;
		private int CurrentIndex { get; set; } = -1;
	}
}
