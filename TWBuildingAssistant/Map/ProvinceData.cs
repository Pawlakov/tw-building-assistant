using System;
using System.Xml;
using System.Globalization;
namespace Map
{
	/// <summary>
	/// Zestaw informacji o pojedyńczej prowincji na mapie.
	/// </summary>
	public class ProvinceData
	{
		private const int _regionsInProvinceCount = 3;
		private readonly RegionData[] _regions;
		/// <summary>
		/// Tworzy nowy zestaw informacji o prowincji.
		/// </summary>
		/// <param name="node">Węzeł XML zawierający potrzebne informacje.</param>
		public ProvinceData(XmlNode node)
		{
			if (node == null)
				throw new ArgumentNullException("node");
			if (node.Name != "province")
				throw new ArgumentException("Given node is not province node.", "node");
			try
			{
				Name = node.Attributes.GetNamedItem("n").InnerText;
			}
			catch (Exception exception)
			{
				throw new FormatException("Could not read province's name.", exception);
			}
			try
			{
				Fertility = Convert.ToInt32(node.Attributes.GetNamedItem("f").InnerText);
			}
			catch (Exception exception)
			{
				throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Could not read fertility level of province {0}.", Name), exception);
			}
			if (Fertility > 6 || Fertility < 1)
				throw (new FormatException(String.Format(CultureInfo.CurrentCulture, "Invalid fertility level in province {0} (is {1}).", Name, Fertility)));
			Climate = ClimateAndWeather.ClimateManager.Singleton.Parse(node.Attributes.GetNamedItem("c").InnerText);
			if (Climate == null)
				throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Could not read climate type of province {0}.", Name));
			//
			XmlNodeList childNodeList = node.ChildNodes;
			if (childNodeList.Count != (_regionsInProvinceCount + 1))
				throw (new FormatException(String.Format(CultureInfo.CurrentCulture, "Incorrect child nodes count in province {0}.", Name)));
			try
			{
				Traditions = new ProvinceTraditions(childNodeList[0]);
			}
			catch (Exception exception)
			{
				throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Failed to create religious traditions set for province {0}.", Name), exception);
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
					throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Failed to create region {0} of province {1}.", whichRegion, Name), excepion);
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
		public ClimateAndWeather.Climate Climate { get; }
		/// <summary>
		/// Zbiór informacji o tradycjach religijnych w tej prowincji.
		/// </summary>
		public ProvinceTraditions Traditions { get; }
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