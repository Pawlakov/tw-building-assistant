namespace TWBuildingAssistant.Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Domain;

public struct Effect
{
    private IEnumerable<Income>? incomes;

    public Effect(int publicOrder = 0, int regularFood = 0, int fertilityDependentFood = 0, int provincialSanitation = 0, int researchRate = 0, int growth = 0, int fertility = 0, int religiousOsmosis = 0, int regionalSanitation = 0, IEnumerable<Income>? incomes = default)
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

    public static Effect operator +(Effect left, Effect right)
    {
        var result = new Effect(
            left.PublicOrder + right.PublicOrder,
            left.RegularFood + right.RegularFood,
            left.FertilityDependentFood + right.FertilityDependentFood,
            left.ProvincialSanitation + right.ProvincialSanitation,
            left.ResearchRate + right.ResearchRate,
            left.Growth + right.Growth,
            left.Fertility + right.Fertility,
            left.ReligiousOsmosis + right.ReligiousOsmosis,
            left.RegionalSanitation + right.RegionalSanitation,
            left.Incomes.Concat(right.Incomes));

        return result;
    }
}