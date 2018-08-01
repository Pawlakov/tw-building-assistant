namespace TWBuildingAssistant.Model.Combinations
{
    using System.Collections.Generic;
    using System.Linq;

    public class Combination
    {
        public Map.ProvinceData Province { get; }

        private readonly BuildingSlot[][] _slots;

        private readonly int[] _sanitation;

        public BuildingSlot[][] Slots
        {
            get
            {
                var result = new BuildingSlot[_slots.Length][];
                int whichSubarray = 0;
                foreach (var subarray in _slots)
                {
                    result[whichSubarray] = subarray.ToArray();
                    ++whichSubarray;
                }
                return result;
            }
        }

        public int[] Sanitation
        {
            get { return _sanitation.ToArray(); }
        }

        public int Food { get; private set; }

        public int PublicOrder { get; private set; }

        public int ReligiousOsmosis { get; private set; }

        public int Fertility { get; private set; }

        public int ResearchRate { get; private set; }

        public int Growth { get; private set; }

        public double Wealth { get; private set; }

        public Combination(Map.ProvinceData province)
        {
            Province = province;
            _slots = new BuildingSlot[3][];
            _sanitation = new int[3];
            for (int whichRegion = 0; whichRegion < 3; ++whichRegion)
            {
                _slots[whichRegion] = new BuildingSlot[Province[whichRegion].SlotsCount];
                for (int whichSlot = 0; whichSlot < _slots[whichRegion].Length; ++whichSlot)
                    _slots[whichRegion][whichSlot] = new BuildingSlot(ConcludeSlotType(Province[whichRegion], whichSlot));
            }
        }

        public void Calculate(Effects.IProvincionalEffect environment)
        {
            var regionalEffects = new Effects.IRegionalEffect[3];
            for (var whichRegion = 0; whichRegion < 3; ++whichRegion)
            {
                Effects.IRegionalEffect sum = new Effects.RegionalEffect();
                foreach (var buildingSlot in this._slots[whichRegion])
                {
                    if (buildingSlot.Level != null)
                    {
                        sum = sum.Aggregate(buildingSlot.Level.Effect);
                    }
                }

                regionalEffects[whichRegion] = sum;
            }

            var combinedEffect = environment.Aggregate(regionalEffects[0].Aggregate(regionalEffects[1].Aggregate(regionalEffects[2])));

            this.Fertility = combinedEffect.Fertility ?? 0;
            this.Food = combinedEffect.Food(this.Fertility);

            this.PublicOrder = combinedEffect.PublicOrder ?? 0;
            this.ReligiousOsmosis = combinedEffect.ReligiousOsmosis ?? 0;
            this.ResearchRate = combinedEffect.ResearchRate ?? 0;
            this.Growth = combinedEffect.Growth ?? 0;
            for (var whichRegion = 0; whichRegion < this._sanitation.Length; ++whichRegion)
            {
                this._sanitation[whichRegion] = (combinedEffect.ProvincionalSanitation ?? 0)
                                                + (regionalEffects[whichRegion].RegionalSanitation ?? 0);
            }

            var religionCalculator = new Effects.InfluenceCalculator(Province.Traditions);
            religionCalculator.AddInfluences(combinedEffect.Influences);

            this.PublicOrder += religionCalculator.PublicOrder();
            this.Wealth = Effects.WealthCalculator.CalculateTotalWealth(combinedEffect.Bonuses, this.Fertility);
        }

        private Buildings.SlotType ConcludeSlotType(Map.RegionData region, int whichSlot)
        {
            if (whichSlot == 0)
            {
                if (region.IsCity)
                    return Buildings.SlotType.CityCenter;
                return Buildings.SlotType.TownCenter;
            }
            if (region.IsCoastal && whichSlot == 1)
                return Buildings.SlotType.Coast;
            if (region.IsCity)
                return Buildings.SlotType.City;
            return Buildings.SlotType.Town;
        }
    }
}