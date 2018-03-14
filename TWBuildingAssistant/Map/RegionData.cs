using System;
using System.Xml;
namespace Map
{
	/// <summary>
	/// Zestaw danych dotyczących jednego regionu prowincji.
	/// </summary>
	public class RegionData
	{
		const int _cityDefaultSlotsCount = 6;
		const int _townDefaultSlotsCount = 4;
		readonly int _slotsCountOffset;
		/// <summary>
		/// Tworzy nowy zestaw informacji o regionie.
		/// </summary>
		/// <param name="node">Węzeł XML zawierający konieczne informacje.</param>
		/// <param name="isCity">Jeżeli prawdziwe, to region posiada duże miasto (w przeciwnym przypadku małe).</param>
		public RegionData(XmlNode node, bool isCity)
		{
			if (node == null)
				throw new MapException("Próbowano utworzyć dane regionu na podstawie pustej wartości.");
			IsCity = isCity;
			Resource = Resource.NONE;
			_slotsCountOffset = 0;
			try
			{
				Name = node.Attributes.GetNamedItem("n").InnerText;
			}
			catch (Exception exception)
			{
				throw new MapException("Nie udało się odczytać nazwy tego regionu.", exception);
			}
			XmlNode temporary;
			temporary = node.Attributes.GetNamedItem("r");
			if (temporary != null)
			{
				try
				{
					Resource = (Resource)Enum.Parse(typeof(Resource), temporary.InnerText);
				}
				catch (Exception exception)
				{
					throw new MapException(String.Format("Nie odczytano nazwy zasobu dla regionu {0}.", Name), exception);
				}
			}
			try
			{
				IsCoastal = Convert.ToBoolean(node.Attributes.GetNamedItem("c").InnerText);
			}
			catch (Exception exception)
			{
				throw new MapException(String.Format("Nie odczytano informacji o dostępie do morza dla regionu {0}.", Name), exception);
			}
			temporary = node.Attributes.GetNamedItem("o");
			if (temporary != null)
			{
				try
				{
					_slotsCountOffset = Convert.ToInt32(temporary.InnerText);
				}
				catch (Exception exception)
				{
					throw new MapException(String.Format("Nie odczytano odchylenia rozmiaru dla regionu {0}.", Name), exception);
				}
			}
		}
		/// <summary>
		/// Nazwa stolicy regionu.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// Specjalny zasób obecny w regionie.
		/// </summary>
		public Resource Resource { get; }
		/// <summary>
		/// Jeśli prawdziwe, to miasto ma możliwość budowy portu.
		/// </summary>
		public bool IsCoastal { get; }
		/// <summary>
		/// Jeśli prawdziwe to region zawiera duże miasto (małe w przeciwnym wypadku).
		/// </summary>
		public bool IsCity { get; }
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
				return result += (_slotsCountOffset);
			}
		}
	}
}