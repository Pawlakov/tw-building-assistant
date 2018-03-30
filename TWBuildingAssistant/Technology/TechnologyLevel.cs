using System;
using System.Linq;
using System.Xml.Linq;
namespace Technologies
{
	public class TechnologyLevel
	{
		// Stan wewnętrzny:
		//
		private ITechnologyTree ContainingTree { get; }
		//
		// Stan wewnętrzny / Interfejs publiczny:
		//
		public int Sanitation { get; private set; }
		public int Fertility { get; private set; }
		public int Growth { get; private set; }
		public int PublicOrder { get; private set; }
		public int ReligiousInfluence { get; private set; }
		public WealthBonuses.WealthBonus[] Bonuses { get; private set; }
		public bool IsAvailable { get; private set; }
		//
		// Interfejs wewnętrzny:
		//
		internal void Cumulate(TechnologyLevel added)
		{
			if (added == null)
				throw new ArgumentNullException("added");
			Sanitation += added.Sanitation;
			Fertility += added.Fertility;
			Growth += added.Growth;
			PublicOrder += added.PublicOrder;
			ReligiousInfluence += added.ReligiousInfluence;
			WealthBonuses.WealthBonus[] newArray = new WealthBonuses.WealthBonus[Bonuses.Length + added.Bonuses.Length];
			try
			{
				Bonuses.CopyTo(newArray, 0);
				added.Bonuses.CopyTo(newArray, Bonuses.Length);
			}
			catch (Exception exception)
			{
				throw new Exception("Failed to cumulate wealth bonuses of technology levels..", exception);
			}
			Bonuses = newArray;
		}
		internal TechnologyLevel(ITechnologyTree containingTree, XElement element)
		{
			ContainingTree = containingTree;
			containingTree.DesiredTechnologyChanged += (ITechnologyTree sender, EventArgs e) => { IsAvailable = containingTree.IsLevelReasearched(this); };
			XAttribute temporary;
			temporary = element.Attribute("s");
			if (temporary != null)
				Sanitation = (int)temporary;
			temporary = element.Attribute("i");
			if (temporary != null)
				Fertility = (int)temporary;
			temporary = element.Attribute("g");
			if (temporary != null)
				Growth = (int)temporary;
			temporary = element.Attribute("po");
			if (temporary != null)
				PublicOrder = (int)temporary;
			temporary = element.Attribute("ri");
			if (temporary != null)
				ReligiousInfluence = (int)temporary;
			Bonuses = (from XElement bonusElement in element.Elements() select WealthBonuses.BonusFactory.MakeBonus(bonusElement)).ToArray();
		}
		internal TechnologyLevel(ITechnologyTree containingTree)
		{
			ContainingTree = containingTree;
			containingTree.DesiredTechnologyChanged += (ITechnologyTree sender, EventArgs e) => { IsAvailable = containingTree.IsLevelReasearched(this); };
		}
	}
}