using System;
using System.Xml.Linq;
using EnumsNET;
namespace GameWorld.Resources
{
	// Obiekty zawierają informacje o zasobach specjalnych.
	public class Resource
	{
		public string Name { get; }
		// Typ slotu w którym powinien się z naleźć budynek wydobywający zasób.
		public ReplacedBuildingType BuildingType { get; }
		// Czy postawienie budynku wydobywającego ten zasób jest konieczne.
		public bool IsMandatory { get; }
		//
		public Resource(XElement element)
		{
			if (element == null)
				throw new ArgumentNullException("element");
			if (!ValidateElement(element, out string message))
				throw new FormatException(message);
			Name = (string)element.Attribute("n");
			BuildingType = Enums.Parse<ReplacedBuildingType>((string)element.Attribute("bt"));
			IsMandatory = (bool)element.Attribute("im");
		}
		//
		public static bool ValidateElement(XElement element, out string message)
		{
			if (element.Attribute("n") == null)
			{
				message = "XML element lacks 'n' attribute.";
				return false;
			}
			if (element.Attribute("bt") == null)
			{
				message = "XML element lacks 'bt' attribute (name: " + (string)element.Attribute("n") + ").";
				return false;
			}
			if (element.Attribute("im") == null)
			{
				message = "XML element lacks 'im' attribute (name: " + (string)element.Attribute("n") + ").";
				return false;
			}
			if (!Enums.TryParse((string)element.Attribute("bt"), out ReplacedBuildingType nothing))
			{
				message = "XML element's 'bt' attribute is not a ReplacedBuildingType value (name: " + (string)element.Attribute("n") + ").";
				return false;
			}
			if (!Boolean.TryParse((string)element.Attribute("im"), out bool irrelevant))
			{
				message = "XML element's 'im' attribute is not a boolean value (name: " + (string)element.Attribute("n") + ").";
				return false;
			}
			message = "XML element is a valid representation of a resource.";
			return true;
		}
		//
		public enum ReplacedBuildingType
		{
			Main,
			General,
			Coast
		}
	}
}
