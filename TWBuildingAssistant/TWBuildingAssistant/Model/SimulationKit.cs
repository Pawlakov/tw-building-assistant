namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;
    using System.Linq;

    public class SimulationKit
    {
        private readonly Effects.IProvincialEffect enivronment;

        private readonly Combinations.Combination combination;

        private readonly List<Buildings.BuildingLevel>[][] availableBuildings;

        public SimulationKit(Buildings.BuildingLibrary library, Combinations.Combination combination)
        {
            this.enivronment = World.GetWorld().Environment;
            this.combination = combination;
            this.availableBuildings = new List<Buildings.BuildingLevel>[this.GetRegionsCount()][];
            for (var whichRegion = 0; whichRegion < this.availableBuildings.Length; ++whichRegion)
            {
                var resource = this.combination.Province.Regions.ElementAt(whichRegion).GetResource();
                this.availableBuildings[whichRegion] = new List<Buildings.BuildingLevel>[this.GetSlotsCount(whichRegion)];
                for (var whichSlot = 0; whichSlot < this.availableBuildings[whichRegion].Length; ++whichSlot)
                {
                    var type = this.combination.Slots[whichRegion][whichSlot].Type;
                    this.availableBuildings[whichRegion][whichSlot] = library.GetLevels(type, resource).ToList();
                }
            }
        }

        public int GetRegionsCount()
        {
            return this.combination.Slots.Length;
        }

        public string ProvinceName()
        {
            return this.combination.Province.Name;
        }

        public string RegionName(int whichRegion)
        {
            return this.combination.Province.Regions.ElementAt(whichRegion).Name;
        }

        public int GetSlotsCount(int whichRegion)
        {
            return this.combination.Slots[whichRegion].Length;
        }

        public void SetBuildingAt(int whichRegion, int whichSlot, int whichBuilding)
        {
            this.combination.Slots[whichRegion][whichSlot].Level = this.availableBuildings[whichRegion][whichSlot][whichBuilding];
        }

        public IEnumerable<KeyValuePair<int, string>> GetAvailableBuildingsAt(int whichRegion, int whichSlot)
        {
            var result = new Dictionary<int, string>();
            var whichBuilding = 0;
            foreach (var building in this.availableBuildings[whichRegion][whichSlot])
            {
                result.Add(whichBuilding, building != null ? building.ToString() : "Empty");
                ++whichBuilding;
            }

            return result;
        }

        public CombinationPerformance CurrentPerformance()
        {
            this.combination.Calculate(this.enivronment);
            var isConflicted = false;
            for (var whichRegion = 0; whichRegion < this.GetRegionsCount(); ++whichRegion)
            {
                for (var whichSlot = 0; whichSlot < this.GetSlotsCount(whichRegion); ++whichSlot)
                {
                    for (var i = whichSlot + 1; i < this.GetSlotsCount(whichRegion); ++i)
                    {
                        if (this.combination.Slots[whichRegion][whichSlot].Level != null
                            && this.combination.Slots[whichRegion][i].Level != null
                            && this.combination.Slots[whichRegion][whichSlot].Level.ContainingBranch
                            == this.combination.Slots[whichRegion][i].Level.ContainingBranch)
                        {
                            isConflicted = true;
                        }
                    }
                }
            }

            return new CombinationPerformance
                   {
                   Sanitation = this.combination.Sanitation.ToArray(),
                   Food = this.combination.Food,
                   PublicOrder = this.combination.PublicOrder,
                   ReligiousOsmosis = this.combination.ReligiousOsmosis,
                   Fertility = this.combination.Fertility,
                   ResearchRate = this.combination.ResearchRate,
                   Growth = this.combination.Growth,
                   Wealth = (int)this.combination.Wealth,
                   Conflict = isConflicted
                   };
        }

        public struct CombinationPerformance
        {
            public int[] Sanitation { get; set; }

            public int Food { get; set; }

            public int PublicOrder { get; set; }

            public int ReligiousOsmosis { get; set; }

            public int Fertility { get; set; }

            public int ResearchRate { get; set; }

            public int Growth { get; set; }

            public int Wealth { get; set; }

            public bool Conflict { get; set; }
        }
    }
}