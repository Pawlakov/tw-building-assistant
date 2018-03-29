using System;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using System.Collections.Generic;
namespace Map
{
	/// <summary>
	/// Zestaw informacji o pojedyńczej prowincji na mapie.
	/// </summary>
	public class ProvinceData
	{
		// Stałe:
		//
		private const int _regionsInProvinceCount = 3;
		private const int _minimalDefaultFretility = 1;
		private const int _maximalDefaultFertility = 6;
		//
		// Interfejs wewnętrzny:
		//
		internal ProvinceData(XElement element)
		{
			Name = (string)element.Attribute("n");
			DefaultFertility = (int)element.Attribute("f");
			if (Fertility > _maximalDefaultFertility || Fertility < _minimalDefaultFretility)
				throw (new FormatException(String.Format(CultureInfo.CurrentCulture, "Invalid fertility level in province {0} (is {1}).", Name, Fertility)));
			Climate = ClimateAndWeather.ClimateManager.Singleton.Parse((string)element.Attribute("c"));
			if (Climate == null)
				throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Could not read climate type of province {0}.", Name));
			Traditions = new ProvinceTraditions(element.Element("traditions"));
			IEnumerable<XElement> regionElements = from XElement regionElement in element.Elements() where regionElement.Name == "region" select regionElement;
			if (regionElements.Count() != _regionsInProvinceCount)
				throw (new FormatException(String.Format(CultureInfo.CurrentCulture, "Incorrect child nodes count in province {0}.", Name)));
			_regions = new RegionData[_regionsInProvinceCount];
			int whichRegion = 0;
			foreach(XElement regionElement in regionElements)
			{
				_regions[whichRegion] = new RegionData(regionElement, !Convert.ToBoolean(whichRegion));
				++whichRegion;
			}
			CurrentFertilityDrop = 0;
			Map.Singleton.FertilityDropChanged += (Map sender, EventArgs e) => { CurrentFertilityDrop = sender.FertilityDrop; };
		}
		//
		// Interfejs publiczny:
		//
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
		/// <summary>
		/// Obecny poziom żyzności prowincji (z zakresu od 0 do 6).
		/// </summary>
		public int Fertility
		{
			get
			{
				if (DefaultFertility - CurrentFertilityDrop < 0)
					return 0;
				return DefaultFertility - CurrentFertilityDrop;
			}
		}
		//
		// Interfejs publiczny / Stan wewnętrzny:
		//
		/// <summary>
		/// Nazwa prowincji.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// Domyślny poziom żyzności prowincji (z zakresu od 1 do 6).
		/// </summary>
		public int DefaultFertility { get; }
		/// <summary>
		/// Typ klimatu panującego w prowincji.
		/// </summary>
		public ClimateAndWeather.Climate Climate { get; }
		/// <summary>
		/// Zbiór informacji o tradycjach religijnych w tej prowincji.
		/// </summary>
		public ProvinceTraditions Traditions { get; }
		//
		// Stan wewnętrzny:
		//
		private readonly RegionData[] _regions;
		private int CurrentFertilityDrop { get; set; }
	}
}