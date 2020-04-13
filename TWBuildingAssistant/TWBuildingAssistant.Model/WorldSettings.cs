namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;

    public class WorldSettings
    {
        public int StateReligionIndex { get; set; }

        public int FertilityDrop { get; set; }

        public IEnumerable<int> ConsideredWeathers { get; set; }

        public int FactionIndex { get; set; }

        public int DesiredTechnologyLevelIndex { get; set; }

        public bool UseLegacyTechnologies { get; set; }

        public int ProvinceIndex { get; set; }
    }
}