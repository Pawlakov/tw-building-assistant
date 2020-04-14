namespace TWBuildingAssistant.Model.Combinations
{
    //using System.Linq;

    //using TWBuildingAssistant.Model.Map;

    //public class Combination
    //{
    //    private readonly BuildingSlot[][] slots;

    //    private readonly int[] sanitation;

    //    public Combination(IProvince province)
    //    {
    //        this.Province = province;
    //        this.sanitation = new int[province.Regions.Count()];
    //        this.slots = province.Regions.Select(
    //            x =>
    //                {
    //                    var region = new BuildingSlot[x.GetSlotsCount()];
    //                    for (var whichSlot = 0; whichSlot < x.GetSlotsCount(); ++whichSlot)
    //                    {
    //                        region[whichSlot] = new BuildingSlot(this.ConcludeSlotType(x, whichSlot));
    //                    }

    //                    return region;
    //                }).ToArray();
    //    }

    //    public IProvince Province { get; }

    //    public BuildingSlot[][] Slots
    //    {
    //        get
    //        {
    //            var result = new BuildingSlot[this.slots.Length][];
    //            var whichSubarray = 0;
    //            foreach (var subarray in this.slots)
    //            {
    //                result[whichSubarray] = subarray.ToArray();
    //                ++whichSubarray;
    //            }

    //            return result;
    //        }
    //    }

    //    public int[] Sanitation => this.sanitation.ToArray();

    //    public int Food { get; private set; }

    //    public int PublicOrder { get; private set; }

    //    public int ReligiousOsmosis { get; private set; }

    //    public int Fertility { get; private set; }

    //    public int ResearchRate { get; private set; }

    //    public int Growth { get; private set; }

    //    public double Wealth { get; private set; }

    //    public void Calculate(Effects.IProvincialEffect environment)
    //    {
    //        var regionalEffects = new Effects.IRegionalEffect[3];
    //        for (var whichRegion = 0; whichRegion < 3; ++whichRegion)
    //        {
    //            Effects.IRegionalEffect sum = new Effects.RegionalEffect();
    //            sum = this.slots[whichRegion].Where(buildingSlot => buildingSlot.Level != null).Aggregate(sum, (current, buildingSlot) => current.Aggregate(buildingSlot.Level.Effect));
    //            regionalEffects[whichRegion] = sum;
    //        }

    //        var combinedEffect = environment.Aggregate(this.Province.Effect.Aggregate(regionalEffects[0].Aggregate(regionalEffects[1].Aggregate(regionalEffects[2]))));

    //        this.Fertility = this.Province.Fertility + combinedEffect.Fertility;
    //        this.Fertility = this.Fertility > 6 ? 6 : this.Fertility;
    //        this.Food = combinedEffect.Food(this.Fertility);

    //        this.PublicOrder = combinedEffect.PublicOrder;
    //        this.ReligiousOsmosis = combinedEffect.ReligiousOsmosis;
    //        this.ResearchRate = combinedEffect.ResearchRate;
    //        this.Growth = combinedEffect.Growth;
    //        for (var whichRegion = 0; whichRegion < this.sanitation.Length; ++whichRegion)
    //        {
    //            this.sanitation[whichRegion] = combinedEffect.ProvincialSanitation
    //                                           + regionalEffects[whichRegion].RegionalSanitation;
    //        }

    //        this.PublicOrder += Effects.InfluenceCalculator.PublicOrder(combinedEffect.Influences);
    //        this.Wealth = Effects.WealthCalculator.CalculateTotalWealth(combinedEffect.Bonuses, this.Fertility);
    //    }

    //    private Buildings.SlotType ConcludeSlotType(IRegion region, int whichSlot)
    //    {
    //        if (whichSlot == 0)
    //        {
    //            return region.IsCity ? Buildings.SlotType.CityCenter : Buildings.SlotType.TownCenter;
    //        }

    //        if (region.IsCoastal && whichSlot == 1)
    //        {
    //            return Buildings.SlotType.Coast;
    //        }

    //        return region.IsCity ? Buildings.SlotType.City : Buildings.SlotType.Town;
    //    }
    //}
}