using System;
using System.Xml.Linq;
namespace GameWorld.Map
{
	// Opis regionu (składowa prowincji) na mapie.
	public class RegionData
	{
		private const int _cityDefaultSlotsCount = 6;
		private const int _townDefaultSlotsCount = 4;
		//
		public RegionData(XElement element, bool isCity, IResourceParser resourceParser)
		{
			if (element == null)
				throw new ArgumentNullException("element");
			if (resourceParser == null)
				throw new ArgumentNullException("resourceParser");
			if (!ValidateElement(element, resourceParser, out string message))
				throw new FormatException(message);
			IsCity = isCity;
			Resource = null;
			SlotsCountOffset = 0;
			Name = (string)element.Attribute("n");
			if (element.Attribute("r") != null)
				Resource = resourceParser.Parse((string)element.Attribute("r"));
			IsCoastal = (bool)element.Attribute("ic");
			if (element.Attribute("o") != null)
				SlotsCountOffset = (int)element.Attribute("o");
		}
		// Możliwe odchylenie od standardowej liczby miejsc na budynki.
		private int SlotsCountOffset { get; }
		// Nazwa regionu.
		public string Name { get; }
		// Zasób mogący się znajdować w regionie.
		public Resources.Resource Resource { get; }
		// Czy region ma dostęp do morza.
		public bool IsCoastal { get; }
		// Czy miasto w regionie jest duże (false - jest małe).
		public bool IsCity { get; }
		//
		public int SlotsCount
		{
			get
			{
				if (IsCity)
					return _cityDefaultSlotsCount + SlotsCountOffset;
				return _townDefaultSlotsCount + SlotsCountOffset;
			}
		}
		// Sprawdzenie czy element XML jest poprawnym opisem regionu.
		public static bool ValidateElement(XElement element, IResourceParser resourceParser, out string message)
		{
			if (element.Attribute("n") == null)
			{
				message = "XML element lacks 'n' attribute.";
				return false;
			}
			if (element.Attribute("ic") == null)
			{
				message = "XML element lacks 'ic' attribute (name: " + (string)element.Attribute("n") + ").";
				return false;
			}
			if (element.Attribute("r") != null)
			{
				if (resourceParser.Parse((string)element.Attribute("r")) == null)
				{
					message = "XML element's 'r' attribute is not a resource name (name: " + (string)element.Attribute("n") + ").";
					return false;
				}
			}
			if (element.Attribute("o") != null)
			{
				if(!Int32.TryParse((string)element.Attribute("o"), out int irrelevant))
				{
					message = "XML element's 'o' attribute is not an integer value (name: " + (string)element.Attribute("n") + ").";
					return false;
				}
			}
			message = "XML element is a valid representation of a region.";
			return true;
		}
	}
}
