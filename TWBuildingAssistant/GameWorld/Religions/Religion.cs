using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
namespace GameWorld.Religions
{
	// Obiekty tej klasy reprezentują religie.
	public class Religion : IReligion
	{
		public Religion(XElement element, Map.IStateReligionTracker stateReligionTracker)
		{
			if (element == null)
				throw new ArgumentNullException("element");
			if (!ValidateElement(element, out string message))
				throw new FormatException(message);
			Name = (string)element.Attribute("n");
			int? temporary = (int?)element.Attribute("po");
			if (temporary.HasValue)
				PublicOrder = temporary.Value;
			temporary = (int?)element.Attribute("g");
			if (temporary.HasValue)
				Growth = temporary.Value;
			temporary = (int?)element.Attribute("rr");
			if (temporary.HasValue)
				ResearchRate = temporary.Value;
			temporary = (int?)element.Attribute("s");
			if (temporary.HasValue)
				Sanitation = temporary.Value;
			temporary = (int?)element.Attribute("ri");
			if (temporary.HasValue)
				ReligiousInfluence = temporary.Value;
			temporary = (int?)element.Attribute("ro");
			if (temporary.HasValue)
				ReligiousOsmosis = temporary.Value;
			if (element.Elements().Count() > 0)
				WealthBonus = WealthBonuses.BonusFactory.MakeBonus(element.Elements().First());
			stateReligionTracker.StateReligionChanged += (Map.IStateReligionTracker sender, EventArgs e) => { _stateReligion = sender.StateReligion; };
		}
		//
		public string Name { get; }
		public int PublicOrder { get; }
		public int Growth { get; }
		public int ResearchRate { get; }
		public int Sanitation { get; }
		public int ReligiousInfluence { get; }
		public int ReligiousOsmosis { get; }
		public int Food { get; }
		public int Fertility { get; }
		public IEnumerable<WealthBonuses.WealthBonus> Bonuses
		{
			get
			{
				if (WealthBonus != null)
					return new WealthBonuses.WealthBonus[] { WealthBonus };
				else
					return new WealthBonuses.WealthBonus[0];
			}
		}
		// Metoda sprawdza czy ta religia jest wybrana jako państwowa.
		public bool IsState
		{
			get
			{
				return this == _stateReligion;
			}
		}
		// Aktualna religia państwowa. Aktualizowana przez event.
		private Religion _stateReligion;
		private WealthBonuses.WealthBonus WealthBonus { get; }
		// Funkcja do walidacji elementu XML jako opisu religii.
		public static bool ValidateElement(XElement element, out string message)
		{
			int dummy;
			int attributeCount = element.Attributes().Count() - 1;
			if (element.Attribute("n") == null)
			{
				--attributeCount;
				message = "XML element lacks 'n' attribute.";
				return false;
			}
			if (element.Attribute("po") != null)
			{
				--attributeCount;
				if (!Int32.TryParse((string)element.Attribute("po"), out dummy))
				{
					message = "XML element's 'po' attribute is not an integer value (name: " + (string)element.Attribute("n") + ").";
					return false;
				}
			}
			if (element.Attribute("g") != null)
			{
				--attributeCount;
				if (!Int32.TryParse((string)element.Attribute("g"), out dummy))
				{
					message = "XML element's 'g' attribute is not an integer value (name: " + (string)element.Attribute("n") + ").";
					return false;
				}
			}
			if (element.Attribute("rr") != null)
			{
				--attributeCount;
				if (!Int32.TryParse((string)element.Attribute("rr"), out dummy))
				{
					message = "XML element's 'rr' attribute is not an integer value (name: " + (string)element.Attribute("n") + ").";
					return false;
				}
			}
			if (element.Attribute("s") != null)
			{
				--attributeCount;
				if (!Int32.TryParse((string)element.Attribute("s"), out dummy))
				{
					message = "XML element's 's' attribute is not an integer value (name: " + (string)element.Attribute("n") + ").";
					return false;
				}
			}
			if (element.Attribute("ri") != null)
			{
				--attributeCount;
				if (!Int32.TryParse((string)element.Attribute("ri"), out dummy))
				{
					message = "XML element's 'ri' attribute is not an integer value (name: " + (string)element.Attribute("n") + ").";
					return false;
				}
			}
			if (element.Attribute("ro") != null)
			{
				--attributeCount;
				if (!Int32.TryParse((string)element.Attribute("ro"), out dummy))
				{
					message = "XML element's 'ro' attribute is not an integer value (name: " + (string)element.Attribute("n") + ").";
					return false;
				}
			}
			if (attributeCount != 0)
			{
				message = "XML element contains unrecognized attributes (name: " + (string)element.Attribute("n") + ").";
				return false;
			}
			if (element.Elements().Count() > 0)
			{
				if (element.Elements().Count() > 1)
				{
					message = "XML element has too many sub-elements (name: " + (string)element.Attribute("n") + ").";
					return false;
				}
				if (!WealthBonuses.BonusFactory.ValidateElement(element.Elements().First(), out string submessage))
				{
					message = "XML element's sub-element is not a valid description of a wealth bonus (name: " + (string)element.Attribute("n") + "): " + submessage;
					return false;
				}
			}
			message = "XML element is a valid representation of a religion.";
			return true;
		}
	}
}