using System;
using System.Linq;
using System.Xml.Linq;
namespace Buildings
{
	public enum BuildingType
	{
		Town,
		CenterTown,
		City,
		CenterCity,
		Coast,
		Resource,
		Spice
	};
	public class Branch
	{
		// Stan wewnętrzny / Interfejs publiczny:
		//
		public string Name { get; }
		public BuildingType Type { get; }
		//
		// Stan wewnętrzny:
		//
		private Building[] BuildingLevels { get; }
		//
		// Interfejs wewnętrzny:
		//
		internal Branch(XElement element, Technologies.ITechnologyTree tree)
		{
			Name = (string)element.Attribute("n");
			Type = (BuildingType)Enum.Parse(typeof(BuildingType), (string)element.Attribute("t"));
			BuildingLevels = (from XElement levelElement in element.Elements() select new Building(levelElement, tree)).ToArray();
		}
		//
		// Interfejs publiczny:
		//
		public virtual Building this[int whichLevel]
		{
			get { return BuildingLevels[whichLevel]; }
		}
		internal virtual Building[] Levels
		{
			get { return BuildingLevels; }
		}
	}
}