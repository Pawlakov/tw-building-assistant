namespace TWBuildingAssistant.Model.Factions
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    using TWBuildingAssistant.Model.Religions;

    public class Faction
    {
        public Faction(XElement element, Resources.IResourceParser resourceParser, IReligionParser religionParser)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            Name = (string)element.Attribute("n");
            TechLevels = Technologies.TechnologyTreeFactory.MakeTechnologyTree((string)element.Attribute("t"));
            Buildings = new Buildings.BuildingLibrary((string)element.Attribute("b"), TechLevels, resourceParser, religionParser);
        }

        public string Name { get; }

        public Effects.IProvincialEffect Effect => TechLevels.CurrentLevel.Effect;

        public void ChangeDesiredTechnologyLevel(int whichLevel, bool useLegacyTechnolgies)
        {
            TechLevels.ChangeDesiredTechnologyLevel(whichLevel, useLegacyTechnolgies);
        }

        public Buildings.BuildingLibrary Buildings { get; }

        private Technologies.ITechnologyTree TechLevels { get; }
    }
}