namespace TWBuildingAssistant.Domain.Models;

using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain.Exceptions;

public static class IncomeOperations
{
    public static Income Create(int value, IncomeCategory? category, BonusType type)
    {
        return (value, category, type) switch
        {
            (0, _, _) =>
                throw new DomainRuleViolationException("'0' income."),
            ( > 0, IncomeCategory.Maintenance, _) =>
                throw new DomainRuleViolationException("Positive 'Maintenance' income."),
            (_, IncomeCategory.Maintenance, not BonusType.Simple) =>
                throw new DomainRuleViolationException("Invalid 'Maintenance' income."),
            (_, null, not BonusType.Percentage) =>
                throw new DomainRuleViolationException("Invalid 'All' income."),
            (_, not IncomeCategory.Husbandry and not IncomeCategory.Agriculture, BonusType.FertilityDependent) =>
                throw new DomainRuleViolationException("Invalid fertility-based income."),
            (_, null, _) =>
                new Income(new KeyValuePair<IncomeCategory, Record>[0], value, 0),
            (_, IncomeCategory.Maintenance, _) =>
                new Income(new KeyValuePair<IncomeCategory, Record>[0], 0, value),
            (_, _, _) =>
                new Income(new [] { new KeyValuePair<IncomeCategory, Record>(category.Value, Create(value, type)) }, 0, 0),
        };
    }

    public static Record Create(int value, BonusType type)
    {
        return type switch
        {
            BonusType.Simple =>
                new Record(value, 0, 0),
            BonusType.Percentage =>
                new Record(0, value, 0),
            BonusType.FertilityDependent =>
                new Record(0, 0, value),
            _ =>
                throw new DomainRuleViolationException("Unknown bonus type."),
        };
    }

    public static double Collect(IEnumerable<Income> incomes, int fertilityLevel)
    {
        var records = new List<KeyValuePair<IncomeCategory, Record>>();
        var allBonus = 0;
        var maintenance = 0;
        foreach (var income in incomes)
        {
            foreach (var record in income.Records)
            {
                var presentRecordIndex = records.FindIndex(x => x.Key == record.Key);
                if (presentRecordIndex > -1)
                {
                    records[presentRecordIndex] = new KeyValuePair<IncomeCategory, Record>(record.Key, Collect(new[] { records[presentRecordIndex].Value, record.Value, }));
                }
                else
                {
                    records.Add(new KeyValuePair<IncomeCategory, Record>(record.Key, record.Value));
                }
            }

            allBonus += income.AllBonus;
            maintenance += income.Maintenance;
        }

        var combined = new Income(records.ToArray(), allBonus, maintenance);
        if (combined.Records == null)
        {
            return 0d;
        }

        var value = 0d;
        foreach (var recordPair in combined.Records)
        {
            var record = recordPair.Value;
            var percentage = (100 + combined.AllBonus + record.Percentage) * 0.01;
            value += (record.Simple + (record.FertilityDependent * fertilityLevel)) * percentage;
        }

        var result = combined.Maintenance + value;
        return result;
    }

    public static Record Collect(IEnumerable<Record> records)
    {
        return new Record(
                records.Sum(x => x.Simple),
                records.Sum(x => x.Percentage),
                records.Sum(x => x.FertilityDependent));
    }

    public static Income TakeWorst(IEnumerable<Income> incomes)
    {
        var oldRecords = incomes
            .SelectMany(x => x.Records)
            .GroupBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Value).ToList());

        var records = new List<KeyValuePair<IncomeCategory, Record>>();
        foreach (var record in oldRecords)
        {
            if (record.Value.Count == 2)
            {
                records.Add(new KeyValuePair<IncomeCategory, Record>(record.Key, TakeWorst(new[] { record.Value[0], record.Value[1] })));
            }
            else
            {
                records.Add(new KeyValuePair<IncomeCategory, Record>(record.Key, TakeWorst(new[] { record.Value[0], default })));
            }
        }

        var allBonus = incomes.Min(x => x.AllBonus);
        var maintenance = incomes.Min(x => x.Maintenance);
        return new Income(records.ToArray(), allBonus, maintenance);
    }

    public static Record TakeWorst(IEnumerable<Record> records)
    {
        return new Record(
            records.Min(x => x.Simple),
            records.Min(x => x.Percentage),
            records.Min(x => x.FertilityDependent));
    }
}

public readonly record struct Income(KeyValuePair<IncomeCategory, Record>[] Records, int Maintenance, int AllBonus);

public readonly record struct Record(int Simple, int Percentage, int FertilityDependent);