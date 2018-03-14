using System;
using System.Xml;

namespace Map
{
	/// <summary>
	/// Zestaw informacji o pojedyńczej prowincji na mapie.
	/// </summary>
	public class ProvinceData
	{
		const int _regionsInProvinceCount = 3;
		readonly RegionData[] _regions;
		/// <summary>
		/// Tworzy nowy zestaw informacji o prowincji.
		/// </summary>
		/// <param name="node">Węzeł XML zawierający potrzebne informacje.</param>
		public ProvinceData(XmlNode node)
		{
			if (node == null)
				throw new MapException("Próbowano utworzyć zestaw danych o prowincji na podstawie pustej wartości.");
			try
			{
				Name = node.Attributes.GetNamedItem("n").InnerText;
			}
			catch (Exception exception)
			{
				throw new MapException("Nie powiodło się odczytanie nazwy dla tej prowincji.", exception);
			}
			try
			{
				Fertility = Convert.ToInt32(node.Attributes.GetNamedItem("f").InnerText);
			}
			catch (Exception exception)
			{
				throw new MapException(String.Format("Nie powiodło się odczytanie żyzności prowincji {0}.", Name), exception);
			}
			if (Fertility > 6 || Fertility < 1)
				throw (new MapException(String.Format("Niewłąciwy poziom żyzności w prowincji {0} (jest {1}).", Name, Fertility)));
			try
			{
				Climate = (Climate)Enum.Parse(typeof(Climate), node.Attributes.GetNamedItem("c").InnerText);
			}
			catch (Exception exception)
			{
				throw new MapException(String.Format("Nie powiodło się odczytanie typu klimatu prowincji {0}.", Name), exception);
			}
			//
			XmlNodeList childNodeList = node.ChildNodes;
			if (childNodeList.Count != 4)
				throw (new MapException(String.Format("Niewłaściwa ilość składników wewnątrz węzła prowincji {0}.", Name)));
			try
			{
				Traditions = new Religion.ProvinceTraditions(childNodeList[0]);
			}
			catch (Exception exception)
			{
				throw new MapException(String.Format("Nie powiodło się tworzenie zbioru tradycji religijnych dla prowincji {0}.", Name), exception);
			}
			//
			_regions = new RegionData[_regionsInProvinceCount];
			for (int whichRegion = 0; whichRegion < _regionsInProvinceCount; ++whichRegion)
			{
				try
				{
					_regions[whichRegion] = new RegionData(childNodeList.Item(whichRegion + 1), !Convert.ToBoolean(whichRegion));
				}
				catch(Exception excepion)
				{
					throw new MapException(String.Format("Nie powiodło się tworzenie {0}-ego regionu w prowincji {1}", whichRegion, Name), excepion);
				}
			}
		}
		/// <summary>
		/// Nazwa prowincji.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// Poziom żyzności prowincji (z zakresu od 1 do 6).
		/// </summary>
		public int Fertility { get; }
		/// <summary>
		/// Typ klimatu panującego w prowincji.
		/// </summary>
		public Climate Climate { get; }
		/// <summary>
		/// Zbiór informacji o tradycjach religijnych w tej prowincji.
		/// </summary>
		public Religion.ProvinceTraditions Traditions { get; }
		/// <summary>
		/// Indekser pozwalający na dostęp do informacji o regionach tej prowincji.
		/// </summary>
		/// <param name="whichRegion">Indeks regionu.</param>
		/// <returns>Zestaw informacji o regionie.</returns>
		public RegionData this[int whichRegion]
		{
			get { return _regions[whichRegion]; }
		}
		/// <summary>
		/// Liczba regionów w prowincji (najprawdopodbniej jest to po prostu 3).
		/// </summary>
		public int RegionsCount
		{
			get { return _regionsInProvinceCount; }
		}
	}
}