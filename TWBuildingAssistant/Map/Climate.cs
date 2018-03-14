using System;
using System.Xml;
namespace Map
{
	public class Climate
	{
		static Climate[] _climates;
		static Weather _worstCaseWeather;
		static Climate()
		{
			XmlDocument sourceFile = new XmlDocument();
			try
			{
				sourceFile.Load(_fileName);
			}
			catch (Exception exception)
			{
				throw new MapException("Nie powiodło się otwarcie pliku opisującego klimaty.", exception);
			}
			XmlNodeList nodeList = sourceFile.GetElementsByTagName("climate");
			_climates = new Climate[nodeList.Count];
			for (int whichClimate = 0; whichClimate < _climates.Length; ++whichClimate)
			{
				try
				{
					_climates[whichClimate] = new Climate(nodeList[whichClimate]);
				}
				catch (Exception exception)
				{
					throw new MapException(String.Format("Nie powiodło się utworzenie obiektu klimatu (na miejscu {0} w pliku).", whichClimate), exception);
				}
			}
			PickWorstCaseWeather();
			PickClimateChangesCount();
		}
		static void PickWorstCaseWeather()
		{
			int choice = -1;
			Console.WriteLine("Which weather should be assumed as 'worst case'?.");
			for (int whichWeather = 0; whichWeather < 3; ++whichWeather)
				Console.WriteLine("{0}. {1}", whichWeather, (Weather)whichWeather);
			while (choice < 0 && choice > 2)
			{
				Console.Write("From 0 to 2: ");
				try
				{
					choice = Convert.ToInt32(Console.ReadLine());
				}
				catch (Exception)
				{
					choice = -1;
				}
			}
			_worstCaseWeather = (Weather)choice;
		}
		static void PickClimateChangesCount()
		{
			ClimateChangesCount = -1;
			Console.WriteLine("Pick how many climate change events already occured.");
			while (ClimateChangesCount < 0 && ClimateChangesCount > 4)
			{
				Console.Write("From 0 to 4: ");
				try
				{
					ClimateChangesCount = Convert.ToInt32(Console.ReadLine());
				}
				catch (Exception)
				{
					ClimateChangesCount = -1;
				}
			}
		}
		public static Climate Parse(string input)
		{
			if (input == null)
				return null;
			foreach (Climate climate in _climates)
			{
				if (climate.Name == input)
					return climate;
			}
			return null;
		}
		public static int ClimateChangesCount { get; private set; }
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
			catch (Exception exception)
			{
				throw new MapException("Nie udało się odczytać nazwy dla opsiu klimatu.", exception);
			}
			if (node.ChildNodes.Count != 3)
				throw new MapException(String.Format("Zła ilość składników węzła XML opisującego klimat {0}.", Name));
			XmlNode extremeNode = node.ChildNodes[0];
			XmlNode badNode = node.ChildNodes[1];
			XmlNode normalNode = node.ChildNodes[2];
			if (extremeNode.Name != "extreme" && badNode.Name != "bad" && normalNode.Name != "normal")
				throw new MapException(String.Format("Niewłaściwe składowe węzła XML opisującego klimat {0}.", Name));
			try
			{
				InitializeArrayForWeather(0, extremeNode);
				InitializeArrayForWeather(1, badNode);
				InitializeArrayForWeather(2, normalNode);
			}
			catch (Exception exception)
			{
				throw new MapException(String.Format("Nie udało się utworzyć opisu pogody dla klimatu {0}.", Name), exception);
			}
		}
		void InitializeArrayForWeather(int index, XmlNode weatherNode)
		{
			try
			{
				_food[index] = Convert.ToInt32(weatherNode.Attributes.GetNamedItem("f").InnerText);
				_publicOrder[index] = Convert.ToInt32(weatherNode.Attributes.GetNamedItem("po").InnerText);
			}
			catch (Exception exception)
			{
				throw new MapException(String.Format("Nie powiodło się tworzenie opisu pogody {1} dla kilimatu {0}.", Name, weatherNode.Name), exception);
			}
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
