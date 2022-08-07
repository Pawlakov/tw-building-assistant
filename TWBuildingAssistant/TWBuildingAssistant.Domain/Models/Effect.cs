namespace TWBuildingAssistant.Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Domain;

public struct Effect
{
    private IEnumerable<Income>? incomes;

    public Effect(int publicOrder, int regularFood, int fertilityDependentFood, int provincialSanitation, int researchRate, int growth, int fertility, int religiousOsmosis, int regionalSanitation, IEnumerable<Income>? incomes)
    {
        this.PublicOrder = publicOrder;
        this.RegularFood = regularFood;
        this.FertilityDependentFood = fertilityDependentFood;
        this.ProvincialSanitation = provincialSanitation;
        this.ResearchRate = researchRate;
        this.Growth = growth;
        this.Fertility = fertility;
        this.ReligiousOsmosis = religiousOsmosis;
        this.RegionalSanitation = regionalSanitation;
        this.incomes = incomes;
    }

    public int PublicOrder { get; }

    public int RegularFood { get; }

    public int FertilityDependentFood { get; }

    public int ProvincialSanitation { get; }

    public int ResearchRate { get; }

    public int Growth { get; }

    public int Fertility { get; }

    public int ReligiousOsmosis { get; }

    public int RegionalSanitation { get; }

    public IEnumerable<Income> Incomes => this.incomes ?? new Income[0];
}

public static class EffectOperations
{
    public static Effect Create(int publicOrder = 0, int regularFood = 0, int fertilityDependentFood = 0, int provincialSanitation = 0, int researchRate = 0, int growth = 0, int fertility = 0, int religiousOsmosis = 0, int regionalSanitation = 0, IEnumerable<Income>? incomes = default)
    {
        return new Effect(publicOrder, regularFood, fertilityDependentFood, provincialSanitation, researchRate, growth, fertility, religiousOsmosis, regionalSanitation, incomes);
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
            effects.Sum(x => x.RegionalSanitation),
            effects.SelectMany(x => x.Incomes));

        return result;
    }
}