﻿namespace TWBuildingAssistant.Model.Buildings
{
    using System.Linq;
    using System.Xml.Linq;

    public class BuildingBranch
    {
        private readonly BuildingLevel[] levels;

        public BuildingBranch(XElement element, ITechnologyLevelAssigner technologyLevelAssigner, Map.IReligionParser religionParser)
        {
            this.Name = (string)element.Attribute("n");
            this.levels = (from XElement subelement in element.Elements() select new BuildingLevel(this, subelement, technologyLevelAssigner)).ToArray();
            this.IsReligiouslyExclusive = false;
            if (element.Attribute("r") == null)
            {
                return;
            }

            this.Religion = religionParser.Parse((string)element.Attribute("r"));
            this.IsReligiouslyExclusive = (bool)element.Attribute("ire");
        }

        public string Name { get; }

        public Religions.IReligion Religion { get; }

        public bool IsReligiouslyExclusive { get; }

        public BuildingLevel[] Levels => this.levels.ToArray();

        public bool IsAvailable
        {
            get
            {
                if (this.levels.All((level) => !level.IsAvailable))
                {
                    return false;
                }

                if (!this.IsReligiouslyExclusive)
                {
                    return true;
                }

                return this.Religion == null || this.Religion.IsState;
            }
        }
    }
}