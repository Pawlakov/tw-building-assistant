using System;
using System.Xml;
namespace Map
{
	public class Climate
	{
		static Climate[] _climates;
		static Weather _worstCaseWeather;
		static int _climateChangesCount;
		static Climate()
		{

		}
		//
		//
		//
		const string _fileName = "twa_climates.xml";
		int[] _publicOrder;
		int[] _food;
		Climate(XmlNode node)
		{
			if (node == null)
				throw new MapException("Próbowano utworzyć opis klimatu na podstawie pustej wartości.");
			try
			{
				Name = node.Attributes.GetNamedItem("n").InnerText;
			}
			catch(Exception exception)
			{
				throw new MapException("Nie udało się odczytać nazwy dla opsiu klimatu.", exception);
			}
			if (node.ChildNodes.Count != 3)
				throw new MapException(String.Format("Zła ilość składników węzła XML opisującego klimat {0}.", Name));
			XmlNode extremeNode;
			XmlNode badNode;
			XmlNode normalNode;
		}
		public string Name { get; }
		public int PublicOrder
		{
			get
			{
				return _publicOrder[(int)_worstCaseWeather];
			}
		}
		public int Food
		{
			get
			{
				return _food[(int)_worstCaseWeather];
			}
		}
		//
		//
		//
		enum Weather { EXTREME, BAD, NORMAL };
	}
}
