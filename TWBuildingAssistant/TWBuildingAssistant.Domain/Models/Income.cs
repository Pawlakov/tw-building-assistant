namespace TWBuildingAssistant.Domain.Models;

using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain.Exceptions;

public readonly record struct Income(IncomeCategory? Category, int Simple, int Percentage, int FertilityDependent);

public static class IncomeOperations
{
    public static Income Create(in int value, IncomeCategory? category, in BonusType type)
    {
        return (value, category, type) switch
        {
            (0, _, _) =>
                throw new DomainRuleViolationException("'0' income."),
            (> 0, IncomeCategory.Maintenance, _) =>
                throw new DomainRuleViolationException("Positive 'Maintenance' income."),
            (_, IncomeCategory.Maintenance, not BonusType.Simple) =>
                throw new DomainRuleViolationException("Invalid 'Maintenance' income."),
            (_, null, not BonusType.Percentage) =>
                throw new DomainRuleViolationException("Invalid 'All' income."),
            (_, not IncomeCategory.Husbandry and not IncomeCategory.Agriculture, BonusType.FertilityDependent) =>
                throw new DomainRuleViolationException("Invalid fertility-based income."),
            (_, _, BonusType.Simple) =>
                new Income(category, value, 0, 0),
            (_, _, BonusType.Percentage) =>
                new Income(category, 0, value, 0),
            (_, _, BonusType.FertilityDependent) =>
                new Income(category, 0, 0, value),
            _ =>
                throw new DomainRuleViolationException("Unknown bonus type."),
        };
    }

    public static double Collect(IEnumerable<Income> incomes, in int fertilityLevel)
    {
        var records = new List<Income>();
        var allBonus = 0;
        foreach (var income in incomes)
        {
            if (income.Category is not null)
            {
                records.Add(income);
            }
            else
            {
                allBonus += income.Percentage;
            }
        }

        var value = 0d;
        foreach (var recordGroup in records.GroupBy(x => x.Category))
        {
            var percentage = 100 + recordGroup.Sum(x => x.Percentage);
            if (recordGroup.Key != IncomeCategory.Maintenance)
            {
                percentage += allBonus;
            }

            value += (recordGroup.Sum(x => x.Simple) + (recordGroup.Sum(x => x.FertilityDependent) * fertilityLevel)) * percentage * 0.01;
        }

        var result = value;
        return result;
    }
}