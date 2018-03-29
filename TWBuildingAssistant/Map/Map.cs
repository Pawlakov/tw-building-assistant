using System;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
namespace Map
{
	/// <summary>
	/// Zawiera i udostępnia informacje o wszystkich prowincjach w grze.
	/// </summary>
	public class Map
	{
		// Singleton:
		//
		private static Map HiddenSingleton { get; set; }
		/// <summary>
		/// Jedna jedyna instancja mapy.
		/// </summary>
		public static Map Singleton
		{
			get
			{
				if (HiddenSingleton == null)
					HiddenSingleton = new Map();
				return HiddenSingleton;
			}
		}
		private Map()
		{
			XDocument sourceDocument = XDocument.Load(_sourceFilename);
			_elements = (from XElement element in sourceDocument.Root.Elements() select element).ToArray();
			_provinces = new ProvinceData[_elements.Count()];
			FertilityDrop = 0;
		}
		//
		// Interfejs wewnętrzny:
		//
		internal event FertilityDropChangingEventHandler FertilityDropChanging;
		internal event FertilityDropChangedEventHandler FertilityDropChanged;
		//
		// Stałe:
		//
		private const string _sourceFilename = "twa_map.xml";
		private const int _minimalFertilityDrop = 0;
		private const int _maximalFertilityDrop = 4;
		//
		// Stan wewnętrzny:
		//
		private readonly ProvinceData[] _provinces;
		private readonly XElement[] _elements;
		//
		// Interfejs publiczny:
		//
		/// <summary>
		/// Indekser udostępniający poszczególne prowincje na mapie.
		/// </summary>
		/// <param name="whichProvince">Indeks żądanej prowincji.</param>
		/// <returns>Dane prowincji po podanym indeksem.</returns>
		public ProvinceData this[int whichProvince]
		{
			get
			{
				if (whichProvince < 0 || whichProvince >= ProvincesCount)
					throw new ArgumentOutOfRangeException("whichProvince", whichProvince, "Index out of range.");
				if (_provinces[whichProvince] == null)
					_provinces[whichProvince] = new ProvinceData(_elements[whichProvince]);
				return _provinces[whichProvince];
			}
		}
		public int ProvincesCount
		{
			get { return _provinces.Length; }
		}
		public override string ToString()
		{
			string result = "Map\n";
			for (int whichProvince = 0; whichProvince < _provinces.Length; ++whichProvince)
				result += String.Format("{0}. {1}\n", whichProvince, (string)_elements[whichProvince].Attribute("n"));
			return result;
		}
		public int FertilityDrop { get; private set; }
		public void ChangeFertilityDrop()
		{
			OnFertilityDropChanging();
			FertilityDrop = -1;
			Console.WriteLine("Pick your current fertility drop:");
			do
			{
				Console.Write("From {0} to {1}: ", _minimalFertilityDrop, _maximalFertilityDrop);
				try
				{
					FertilityDrop = Convert.ToInt32(Console.ReadLine(), CultureInfo.InvariantCulture);
				}
				catch (Exception)
				{
					FertilityDrop = -1;
				}
			} while (FertilityDrop < _minimalFertilityDrop || FertilityDrop > _maximalFertilityDrop);
			OnFertilityDropChanged();
		}
		//
		// Pomocnicze:
		//
		private void OnFertilityDropChanged()
		{
			FertilityDropChanged?.Invoke(this, EventArgs.Empty);
		}
		private void OnFertilityDropChanging()
		{
			FertilityDropChanging?.Invoke(this, EventArgs.Empty);
		}
	}
	internal delegate void FertilityDropChangingEventHandler(Map sender, EventArgs e);
	internal delegate void FertilityDropChangedEventHandler(Map sender, EventArgs e);
}