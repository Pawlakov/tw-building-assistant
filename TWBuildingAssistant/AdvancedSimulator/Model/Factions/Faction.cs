namespace TWBuildingAssistant.Model.Factions
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class Faction
    {
        public Faction(XElement element, Map.IResourceParser resourceParser, Map.IReligionParser religionParser)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            Name = (string)element.Attribute("n");
            TechLevels = Technologies.TechnologyTreeFactory.MakeTechnologyTree((string)element.Attribute("t"));
            Buildings = new Buildings.BuildingLibrary((string)element.Attribute("b"), TechLevels, resourceParser, religionParser);
        }

        public string Name { get; }

        public int Sanitation
        {
            get { return TechLevels.CurrentLevel.Sanitation; }
        }

        public int Fertility
        {
            get { return TechLevels.CurrentLevel.Fertility; }
        }

        public int Growth
        {
            get { return TechLevels.CurrentLevel.Growth; }
        }

        public int PublicOrder
        {
            get { return TechLevels.CurrentLevel.PublicOrder; }
        }

        public int ReligiousInfluence
        {
            get { return TechLevels.CurrentLevel.ReligiousInfluence; }
        }

        public int Food
        {
            get { return TechLevels.CurrentLevel.Food; }
        }

        public int ReligiousOsmosis
        {
            get { return TechLevels.CurrentLevel.ReligiousOsmosis; }
        }

        public int ResearchRate
        {
            get { return TechLevels.CurrentLevel.ResearchRate; }
        }

        public IEnumerable<Effects.WealthBonus> Bonuses
        {
            get { return TechLevels.CurrentLevel.Bonuses; }
        }

        public void ChangeDesiredTechnologyLevel(int whichLevel, bool useLegacyTechnolgies)
        {
            TechLevels.ChangeDesiredTechnologyLevel(whichLevel, useLegacyTechnolgies);
        }

        public Buildings.BuildingLibrary Buildings { get; }

        private Technologies.ITechnologyTree TechLevels { get; }
    }
}