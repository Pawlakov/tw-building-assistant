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

        public Effects.IProvincionalEffect Effect => TechLevels.CurrentLevel.Effect;

        public void ChangeDesiredTechnologyLevel(int whichLevel, bool useLegacyTechnolgies)
        {
            TechLevels.ChangeDesiredTechnologyLevel(whichLevel, useLegacyTechnolgies);
        }

        public Buildings.BuildingLibrary Buildings { get; }

        private Technologies.ITechnologyTree TechLevels { get; }
    }
}