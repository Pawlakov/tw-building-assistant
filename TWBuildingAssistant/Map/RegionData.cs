using System;
using System.Xml;
namespace Map
{
	/// <summary>
	/// Zestaw danych dotyczących jednego regionu prowincji.
	/// </summary>
	public class RegionData
	{
		private const int _cityDefaultSlotsCount = 6;
		private const int _townDefaultSlotsCount = 4;
		private int SlotsCountOffset { get; }
		/// <summary>
		/// Tworzy nowy zestaw informacji o regionie.
		/// </summary>
		/// <param name="node">Węzeł XML zawierający konieczne informacje.</param>
		/// <param name="isCity">Jeżeli prawdziwe, to region posiada duże miasto (w przeciwnym przypadku małe).</param>
		public RegionData(XmlNode node, bool isCity)
		{
			if (node == null)
				throw new ArgumentNullException("node");
			if (node.Name != "region")
				throw new ArgumentException("Given node is not region node.", "node");
			IsCity = isCity;
			Resource = null;
			SlotsCountOffset = 0;
			try
			{
				Name = node.Attributes.GetNamedItem("n").InnerText;
			}
			catch (Exception exception)
			{
				throw new FormatException("Could not read name attribute for this region.", exception);
			}
			XmlNode temporary;
			temporary = node.Attributes.GetNamedItem("r");
			if (temporary != null)
			{
				Resource = Resources.ResourcesManager.Singleton.Parse(temporary.InnerText);
				if(Resource == null)
					throw new FormatException(String.Format("Could not recognize resource type of region {0}.", Name));
			}
			try
			{
				IsCoastal = Convert.ToBoolean(node.Attributes.GetNamedItem("c").InnerText);
			}
			catch (Exception exception)
			{
				throw new FormatException(String.Format("Could not read 'is coastal' attribute of region {0}.", Name), exception);
			}
			temporary = node.Attributes.GetNamedItem("o");
			if (temporary != null)
			{
				try
				{
					SlotsCountOffset = Convert.ToInt32(temporary.InnerText);
				}
				catch (Exception exception)
				{
					throw new FormatException(String.Format("Could not read slots count offset of region {0}.", Name), exception);
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
		public Resources.Resource Resource { get; }
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
				return result + SlotsCountOffset;
			}
		}
	}
}