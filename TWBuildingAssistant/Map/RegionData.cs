using System;
using System.Xml.Linq;
namespace Map
{
	/// <summary>
	/// Zestaw danych dotyczących jednego regionu prowincji.
	/// </summary>
	public class RegionData
	{
		// Stałe:
		//
		private const int _cityDefaultSlotsCount = 6;
		private const int _townDefaultSlotsCount = 4;
		//
		// Interfejs wewnętrzny:
		//
		internal RegionData(XElement element, bool isCity)
		{
			IsCity = isCity;
			Resource = null;
			SlotsCountOffset = 0;
			Name = (string)element.Attribute("n");
			if (element.Attribute("r") != null)
			{
				Resource = Resources.ResourcesManager.Singleton.Parse((string)element.Attribute("r"));
				if (Resource == null)
					throw new FormatException(String.Format("Could not recognize resource type of region {0}.", Name));
			}
			IsCoastal = (bool)element.Attribute("c");
			if (element.Attribute("o") != null)
				SlotsCountOffset = (int)element.Attribute("o");
		}
		//
		// Stan wewnętrzny:
		//
		private int SlotsCountOffset { get; }
		//
		// Stan wewnętrzny / Interfejs publiczny:
		//
		/// <summary>
		/// Nazwa stolicy regionu.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// Specjalny zasób obecny w regionie.
		/// </summary>
		public Resources.Resource Resource { get; }
		/// <summary>
		/// Jeśli prawdziwe, to miasto ma możliwość budowy portu.
		/// </summary>
		public bool IsCoastal { get; }
		/// <summary>
		/// Jeśli prawdziwe to region zawiera duże miasto (małe w przeciwnym wypadku).
		/// </summary>
		public bool IsCity { get; }
		//
		// Interfejs publiczny:
		//
		/// <summary>
		/// Liczba slotów budowlanych w stolicy regionu.
		/// </summary>
		public int SlotsCount
		{
			get
			{
				int result;
				if (IsCity)
					result = _cityDefaultSlotsCount;
				else
					result = _townDefaultSlotsCount;
				return result + SlotsCountOffset;
			}
		}
	}
}