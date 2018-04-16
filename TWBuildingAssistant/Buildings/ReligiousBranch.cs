using System;
using System.Linq;
using System.Xml.Linq;
namespace Buildings
{
	class ReligiousBranch : Branch
	{
		// Stan wewnętrzny:
		//
		private ReligiousBuilding[] ReligiousLevels { get; }
		//
		// Interfejs wewnętrzny:
		//
		internal ReligiousBranch(XElement element, Technologies.ITechnologyTree tree) : base(element, tree) // Konflikt o poziomy
		{
			Religion = Religions.ReligionsManager.Singleton.Parse((string)element.Attribute("r"));
			IsReligiouslyExclusive = (bool)element.Attribute("ire");
			ReligiousLevels = (from XElement levelElement in element.Elements() select new ReligiousBuilding(levelElement, tree, Religion, IsReligiouslyExclusive)).ToArray();
		}
		//
		// Stan wewnętrzny / Interfejs publiczny:
		//
		public Religions.IReligion Religion { get; }
		public bool IsReligiouslyExclusive { get; }
		//
		// Interfejs publiczny:
		//
		public override Building this[int whichLevel]
		{
			get { return ReligiousLevels[whichLevel]; }
		}
		internal override Building[] Levels
		{
			get { return ReligiousLevels; }
		}
	}
}
