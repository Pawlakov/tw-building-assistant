using System;
using System.Linq;
using System.Xml.Linq;
namespace Buildings
{
	public class ReligiousBuilding : Building
	{
		// Interfejs wewnętrzny:
		//
		internal ReligiousBuilding(XElement element, Technologies.ITechnologyTree tree, Religions.IReligion religion, bool isReligiouslyExclusive) : base(element, tree)
		{
			Religion = religion;
			XAttribute temporary = element.Attribute("ri");
			if (temporary != null)
				ReligiousInfluence = (int)temporary;
			temporary = element.Attribute("ro");
			if (temporary != null)
				ReligiousOsmosis = (int)temporary;
			IsReligiouslyExclusive = isReligiouslyExclusive;
			CurrentStateReligion = Religions.ReligionsManager.Singleton.StateReligion;
			Religions.ReligionsManager.Singleton.StateReligionChanged += (Religions.ReligionsManager sender, EventArgs e) => { CurrentStateReligion = sender.StateReligion; };
		}
		//
		// Interfejs publiczny / Stan wewnętrzny:
		//
		public int ReligiousInfluence { get; }
		public int ReligiousOsmosis { get; }
		public Religions.IReligion Religion { get; }
		//
		// Interfejs publiczny:
		//
		public override bool IsAvailable()
		{
			if (IsReligiouslyExclusive)
				return (CurrentStateReligion == Religion) && base.IsAvailable();
			else
				return base.IsAvailable();
		}
		//
		// Stan wewnętrzny:
		//
		private bool IsReligiouslyExclusive { get; }
		private Religions.Religion CurrentStateReligion { get; set; }
	}
}
