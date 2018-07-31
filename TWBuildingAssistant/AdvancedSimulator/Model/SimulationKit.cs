namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;
    using System.Linq;

    public class SimulationKit
    {
        private readonly Effects.IProvincionalEffect _enivronment;

        private readonly Combinations.Combination _combination;

        private readonly List<Buildings.BuildingLevel>[][] _availableBuildings;

        public SimulationKit(World world, Buildings.BuildingLibrary library, Combinations.Combination combination)
        {
            _enivronment = world.Environment;
            _combination = combination;
            _availableBuildings = new List<Buildings.BuildingLevel>[GetRegionsCount()][];
            for (int whichRegion = 0; whichRegion < _availableBuildings.Length; ++whichRegion)
            {
                Resources.IResource resource = _combination.Province[whichRegion].Resource;
                _availableBuildings[whichRegion] = new List<Buildings.BuildingLevel>[GetSlotsCount(whichRegion)];
                for (int whichSlot = 0; whichSlot < _availableBuildings[whichRegion].Length; ++whichSlot)
                {
                    Buildings.SlotType type = _combination.Slots[whichRegion][whichSlot].Type;
                    _availableBuildings[whichRegion][whichSlot] = library.GetLevels(type, resource).ToList();
                }
            }
        }

        public int GetRegionsCount()
        {
            return _combination.Slots.Length;
        }

        public string ProvinceName()
        {
            return _combination.Province.Name;
        }

        public string RegionName(int whichRegion)
        {
            return _combination.Province[whichRegion].Name;
        }

        public int GetSlotsCount(int whichRegion)
        {
            return _combination.Slots[whichRegion].Length;
        }

        public void SetBuildingAt(int whichRegion, int whichSlot, int whichBuilding)
        {
            _combination.Slots[whichRegion][whichSlot].Level = _availableBuildings[whichRegion][whichSlot][whichBuilding];
        }

        public IEnumerable<KeyValuePair<int, string>> GetAvailableBuildingsAt(int whichRegion, int whichSlot)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            int whichBuilding = 0;
            foreach (Buildings.BuildingLevel building in _availableBuildings[whichRegion][whichSlot])
            {
                if (building != null)
                    result.Add(whichBuilding, building.ToString());
                else
                    result.Add(whichBuilding, "Empty");
                ++whichBuilding;
            }
            return result;
        }

        public CombinationPerformance CurrentPerformance()
        {
            // Sprawdzenie czy w kombinacji nie występują konflikty (w jednym regionie nie mogą być 2 lub więcej budynki z tej samej gałęzi).
            _combination.Calculate(_enivronment);
            bool isConflicted = false;
            for (int whichRegion = 0; whichRegion < GetRegionsCount(); ++whichRegion)
                for (int whichSlot = 0; whichSlot < GetSlotsCount(whichRegion); ++whichSlot)
                    for (int i = whichSlot + 1; i < GetSlotsCount(whichRegion); ++i)
                        if (
                            _combination.Slots[whichRegion][whichSlot].Level != null &&
                            _combination.Slots[whichRegion][i].Level != null &&
                            _combination.Slots[whichRegion][whichSlot].Level.ContainingBranch == _combination.Slots[whichRegion][i].Level.ContainingBranch
                            )
                            isConflicted = true;
            //
            return new CombinationPerformance()
            {
                Sanitation = _combination.Sanitation.ToArray(),
                Food = _combination.Food,
                PublicOrder = _combination.PublicOrder,
                ReligiousOsmosis = _combination.ReligiousOsmosis,
                Fertility = _combination.Fertility,
                ResearchRate = _combination.ResearchRate,
                Growth = _combination.Growth,
                Wealth = (int)_combination.Wealth,
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