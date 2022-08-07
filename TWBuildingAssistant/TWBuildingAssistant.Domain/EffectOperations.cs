namespace TWBuildingAssistant.Domain;

using System.Collections.Generic;
using System.Linq;

public static class EffectOperations
{
    public static Effect Create(in int publicOrder = 0, in int regularFood = 0, in int fertilityDependentFood = 0, in int provincialSanitation = 0, in int researchRate = 0, in int growth = 0, in int fertility = 0, in int religiousOsmosis = 0, in int regionalSanitation = 0)
    {
        return new Effect(publicOrder, regularFood, fertilityDependentFood, provincialSanitation, researchRate, growth, fertility, religiousOsmosis, regionalSanitation);
    }

    public static Effect Collect(IEnumerable<Effect> effects)
    {
        var result = new Effect(
            effects.Sum(x => x.PublicOrder),
            effects.Sum(x => x.RegularFood),
            effects.Sum(x => x.FertilityDependentFood),
            effects.Sum(x => x.ProvincialSanitation),
            effects.Sum(x => x.ResearchRate),
            effects.Sum(x => x.Growth),
            effects.Sum(x => x.Fertility),
            effects.Sum(x => x.ReligiousOsmosis),
            effects.Sum(x => x.RegionalSanitation));

        return result;
    }
}