namespace TWBuildingAssistant.Model.Buildings
{
    using System.Linq;
    using System.Xml.Linq;

    using TWBuildingAssistant.Model.Religions;

    public class BuildingBranch
    {
        private readonly BuildingLevel[] levels;

        public BuildingBranch(XElement element, ITechnologyLevelAssigner technologyLevelAssigner, IParser<IReligion> religionParser)
        {
            this.Name = (string)element.Attribute("n");
            this.levels = (from XElement subelement in element.Elements() select new BuildingLevel(this, subelement, technologyLevelAssigner)).ToArray();
            foreach (var level in this.levels)
            {
                foreach (var influence in level.Effect.Influences)
                {
                    influence.ReligionParser = religionParser;
                }
            }

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