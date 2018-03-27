using System;
using System.Linq;
using System.Xml.Linq;
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
		internal Climate(XElement element)
		{
			Name = (string)element.Attribute("n");
			_publicOrder = (from XElement weatherElement in element.Elements() select (int)weatherElement.Attribute("po")).ToArray();
			_food = (from XElement weatherElement in element.Elements() select (int)weatherElement.Attribute("f")).ToArray();
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
