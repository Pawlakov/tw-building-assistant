﻿namespace TWBuildingAssistant.Model.Technologies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class TechnologyLevel
    {
        private ITechnologyTree ContainingTree { get; }

        public int Sanitation { get; private set; }

        public int Fertility { get; private set; }

        public int Growth { get; private set; }

        public int PublicOrder { get; private set; }

        public int ReligiousInfluence { get; private set; }

        public int Food { get; private set; }

        public int ReligiousOsmosis { get; private set; }

        public int ResearchRate { get; private set; }

        public IEnumerable<Effects.WealthBonus> Bonuses { get; private set; }

        public bool IsAvailable { get; private set; }

        public void Cumulate(TechnologyLevel added)
        {
            if (added == null)
                throw new ArgumentNullException("added");
            Sanitation += added.Sanitation;
            Fertility += added.Fertility;
            Growth += added.Growth;
            PublicOrder += added.PublicOrder;
            ReligiousInfluence += added.ReligiousInfluence;
            Bonuses = Bonuses.Concat(added.Bonuses);
        }

        public TechnologyLevel(ITechnologyTree containingTree, XElement element)
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
            temporary = element.Attribute("ro");
            if (temporary != null)
                ReligiousOsmosis = (int)temporary;
            temporary = element.Attribute("f");
            if (temporary != null)
                Food = (int)temporary;
            temporary = element.Attribute("rr");
            if (temporary != null)
                ResearchRate = (int)temporary;
            Bonuses = from XElement bonusElement in element.Elements() select Effects.WealthBonusesFactory.MakeBonus(bonusElement);
        }

        public TechnologyLevel(ITechnologyTree containingTree)
        {
            ContainingTree = containingTree;
            containingTree.DesiredTechnologyChanged += (ITechnologyTree sender, EventArgs e) => { IsAvailable = containingTree.IsLevelReasearched(this); };
            Bonuses = new Effects.WealthBonus[0];
        }
    }
}