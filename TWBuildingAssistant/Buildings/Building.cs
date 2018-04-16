using System.Linq;
using System.Xml.Linq;
namespace Buildings
{
	public class Building
	{
		// Interfejs wewnętrzny:
		//
		internal Building(XElement element, Technologies.ITechnologyTree tree)
		{
			Name = (string)element.Attribute("n");
			Level = (int)element.Attribute("l");
			Technology = tree.Parse((int)element.Attribute("t"), (bool?)element.Attribute("il"));
			//
			XAttribute temporary;
			temporary = element.Attribute("f");
			if (temporary != null)
				Food = (int)temporary;
			temporary = element.Attribute("i");
			if (temporary != null)
				Irrigation = (int)temporary;
			temporary = element.Attribute("fpf");
			if (temporary != null)
				FoodPerFertility = (int)temporary;
			temporary = element.Attribute("po");
			if (temporary != null)
				PublicOrder = (int)temporary;
			temporary = element.Attribute("g");
			if (temporary != null)
				Growth = (int)temporary;
			temporary = element.Attribute("rr");
			if (temporary != null)
				ResearchRate = (int)temporary;
			temporary = element.Attribute("rs");
			if (temporary != null)
				RegionalSanitation = (int)temporary;
			temporary = element.Attribute("ps");
			if (temporary != null)
				ProvincionalSanitation = (int)temporary;
			Bonuses = (from XElement bonusElement in element.Elements() select WealthBonuses.BonusFactory.MakeBonus(bonusElement)).ToArray();
		}
		//
		// Stan wewnętrzny / Interfejs publiczny:
		//
		/// <summary>
		/// Nazwa tego budynku.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// Poziom tego budynku wewnątrz gałęzi budynków.
		/// </summary>
		public int Level { get; }
		public int Irrigation { get; }
		public int PublicOrder { get; }
		public int Growth { get; }
		public int ResearchRate { get; }
		public WealthBonuses.WealthBonus[] Bonuses { get; }
		public int RegionalSanitation { get; }
		public int ProvincionalSanitation { get; }
		//
		// Stan wewnętrzny:
		//
		private int Food { get; }
		private int FoodPerFertility { get; }
		public Technologies.TechnologyLevel Technology { get; }
		//
		// Interfejs publiczny:
		//
		/// <summary>
		/// Zwraca żywność zapewnianą przez ten budynek dla podanego poziomu żyzności.
		/// </summary>
		/// <param name="fertility">Poziom żyzności.</param>
		public int GetFood(int fertility)
		{
			return Food + fertility * FoodPerFertility;
		}
		public virtual bool IsAvailable()
		{
			return Technology.IsAvailable;
		}
	}
}