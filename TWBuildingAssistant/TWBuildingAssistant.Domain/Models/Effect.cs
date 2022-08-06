namespace TWBuildingAssistant.Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;

public struct Effect : IEquatable<Effect>
{
    private IEnumerable<Income>? incomes;

    public Effect(int publicOrder = 0, int regularFood = 0, int fertilityDependentFood = 0, int provincialSanitation = 0, int researchRate = 0, int growth = 0, int fertility = 0, int religiousOsmosis = 0, int regionalSanitation = 0, IEnumerable<Income>? incomes = default, Influence influence = default)
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
        this.Influence = influence;
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

    public Influence Influence { get; }

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
            left.Incomes.Concat(right.Incomes),
            left.Influence + right.Influence);

        return result;
    }

    public static Effect TakeWorse(Effect left, Effect right)
    {
        var result = new Effect(
            Math.Min(left.PublicOrder, right.PublicOrder),
            Math.Min(left.RegularFood, right.RegularFood),
            Math.Min(left.FertilityDependentFood, right.FertilityDependentFood),
            Math.Min(left.ProvincialSanitation, right.ProvincialSanitation),
            Math.Min(left.ResearchRate, right.ResearchRate),
            Math.Min(left.Growth, right.Growth),
            Math.Min(left.Fertility, right.Fertility),
            Math.Min(left.ReligiousOsmosis, right.ReligiousOsmosis),
            Math.Min(left.RegionalSanitation, right.RegionalSanitation),
            new[] { IncomeOperations.TakeWorst(left.Incomes.Concat(right.Incomes)), },
            Influence.TakeWorse(left.Influence, right.Influence));

        return result;
    }

    public bool Equals(Effect other)
    {
        throw new NotImplementedException("When is this thing even used?");
        /*return this.PublicOrder == other.PublicOrder &&
            this.RegularFood == other.RegularFood &&
            this.FertilityDependentFood == other.FertilityDependentFood &&
            this.ProvincialSanitation == other.ProvincialSanitation &&
            this.ResearchRate == other.ResearchRate &&
            this.Growth == other.Growth &&
            this.Fertility == other.Fertility &&
            this.ReligiousOsmosis == other.ReligiousOsmosis &&
            this.RegionalSanitation == other.RegionalSanitation &&
            this.Income.Equals(other.Income) &&
            this.Influence.Equals(other.Influence);*/
    }
}