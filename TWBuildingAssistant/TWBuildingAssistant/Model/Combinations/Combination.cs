namespace TWBuildingAssistant.Model.Combinations
{
    using System.Linq;

    public class Combination
    {
        private readonly BuildingSlot[][] slots;

        private readonly int[] sanitation;

        public Combination(Map.ProvinceData province)
        {
            this.Province = province;
            this.slots = new BuildingSlot[3][];
            this.sanitation = new int[3];
            for (var whichRegion = 0; whichRegion < 3; ++whichRegion)
            {
                this.slots[whichRegion] = new BuildingSlot[this.Province[whichRegion].SlotsCount];
                for (var whichSlot = 0; whichSlot < this.slots[whichRegion].Length; ++whichSlot)
                {
                    this.slots[whichRegion][whichSlot] =
                    new BuildingSlot(this.ConcludeSlotType(this.Province[whichRegion], whichSlot));
                }
            }
        }

        public Map.ProvinceData Province { get; }

        public BuildingSlot[][] Slots
        {
            get
            {
                var result = new BuildingSlot[this.slots.Length][];
                var whichSubarray = 0;
                foreach (var subarray in this.slots)
                {
                    result[whichSubarray] = subarray.ToArray();
                    ++whichSubarray;
                }

                return result;
            }
        }

        public int[] Sanitation => this.sanitation.ToArray();

        public int Food { get; private set; }

        public int PublicOrder { get; private set; }

        public int ReligiousOsmosis { get; private set; }

        public int Fertility { get; private set; }

        public int ResearchRate { get; private set; }

        public int Growth { get; private set; }

        public double Wealth { get; private set; }

        public void Calculate(Effects.IProvincionalEffect environment)
        {
            var regionalEffects = new Effects.IRegionalEffect[3];
            for (var whichRegion = 0; whichRegion < 3; ++whichRegion)
            {
                Effects.IRegionalEffect sum = new Effects.RegionalEffect();
                sum = this.slots[whichRegion].Where(buildingSlot => buildingSlot.Level != null).Aggregate(sum, (current, buildingSlot) => current.Aggregate(buildingSlot.Level.Effect));
                regionalEffects[whichRegion] = sum;
            }

            var combinedEffect = environment.Aggregate(regionalEffects[0].Aggregate(regionalEffects[1].Aggregate(regionalEffects[2])));

            this.Fertility = combinedEffect.Fertility ?? 0;
            this.Food = combinedEffect.Food(this.Fertility);

            this.PublicOrder = combinedEffect.PublicOrder ?? 0;
            this.ReligiousOsmosis = combinedEffect.ReligiousOsmosis ?? 0;
            this.ResearchRate = combinedEffect.ResearchRate ?? 0;
            this.Growth = combinedEffect.Growth ?? 0;
            for (var whichRegion = 0; whichRegion < this.sanitation.Length; ++whichRegion)
            {
                this.sanitation[whichRegion] = (combinedEffect.ProvincionalSanitation ?? 0)
                                               + (regionalEffects[whichRegion].RegionalSanitation ?? 0);
            }

            this.PublicOrder += Effects.InfluenceCalculator.PublicOrder(combinedEffect.Influences.Concat(this.Province.Traditions.Influences));
            this.Wealth = Effects.WealthCalculator.CalculateTotalWealth(combinedEffect.Bonuses, this.Fertility);
        }

        private Buildings.SlotType ConcludeSlotType(Map.RegionData region, int whichSlot)
        {
            if (whichSlot == 0)
            {
                return region.IsCity ? Buildings.SlotType.CityCenter : Buildings.SlotType.TownCenter;
            }

            if (region.IsCoastal && whichSlot == 1)
            {
                return Buildings.SlotType.Coast;
            }

            return region.IsCity ? Buildings.SlotType.City : Buildings.SlotType.Town;
        }
    }
}