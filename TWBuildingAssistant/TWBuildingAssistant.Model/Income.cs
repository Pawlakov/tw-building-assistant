namespace TWBuildingAssistant.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using EnumsNET;
using TWBuildingAssistant.Data.Model;

public struct Income : IEquatable<Income>
{
    private readonly IEnumerable<KeyValuePair<IncomeCategory, IncomeRecord>> records;

    private readonly int maintenance;

    private readonly int allBonus;

    public Income(int value, IncomeCategory? category, BonusType type)
    {
        if (value == 0)
        {
            throw new DomainRuleViolationException("'0' income.");
        }

        if (value > 0 && category == IncomeCategory.Maintenance)
        {
            throw new DomainRuleViolationException("Positive 'Maintenance' income.");
        }

        if (category == IncomeCategory.Maintenance && type != BonusType.Simple)
        {
            throw new DomainRuleViolationException("Invalid 'Maintenance' income.");
        }

        if (category == null && type != BonusType.Percentage)
        {
            throw new DomainRuleViolationException("Invalid 'All' income.");
        }

        if (type == BonusType.FertilityDependent && category != IncomeCategory.Husbandry && category != IncomeCategory.Agriculture)
        {
            throw new DomainRuleViolationException("Invalid fertility-based income.");
        }

        if (category == null)
        {
            this.records = new List<KeyValuePair<IncomeCategory, IncomeRecord>>();
            this.allBonus = value;
            this.maintenance = 0;
        }
        else if (category == IncomeCategory.Maintenance)
        {
            this.records = new List<KeyValuePair<IncomeCategory, IncomeRecord>>();
            this.allBonus = 0;
            this.maintenance = value;
        }
        else
        {
            this.records = new List<KeyValuePair<IncomeCategory, IncomeRecord>> { new KeyValuePair<IncomeCategory, IncomeRecord>(category.Value, new IncomeRecord(value, type)) };
            this.allBonus = 0;
            this.maintenance = 0;
        }
    }

    private Income(IEnumerable<KeyValuePair<IncomeCategory, IncomeRecord>> records, int allBonus, int maintenance)
    {
        this.allBonus = allBonus;
        this.maintenance = maintenance;
        if (records != null)
        {
            this.records = records;
        }
        else
        {
            this.records = new Dictionary<IncomeCategory, IncomeRecord>();
        }
    }

    public static Income operator +(Income left, Income right)
    {
        var records = new List<KeyValuePair<IncomeCategory, IncomeRecord>>();
        if (left.records != null)
        {
            records.AddRange(left.records);
        }

        if (right.records != null)
        {
            foreach (var record in right.records)
            {
                var presentRecordIndex = records.FindIndex(x => x.Key == record.Key);
                if (presentRecordIndex > -1)
                {
                    records[presentRecordIndex] = new KeyValuePair<IncomeCategory, IncomeRecord>(record.Key, records[presentRecordIndex].Value + record.Value);
                }
                else
                {
                    records.Add(new KeyValuePair<IncomeCategory, IncomeRecord>(record.Key, record.Value));
                }
            }
        }

        var allBonus = left.allBonus + right.allBonus;
        var maintenance = left.maintenance + right.maintenance;
        return new Income(records, allBonus, maintenance);
    }

    public static Income TakeWorse(Income left, Income right)
    {
        var oldRecords = (left.records?.ToList() ?? new List<KeyValuePair<IncomeCategory, IncomeRecord>>())
            .Concat(right.records?.ToList() ?? new List<KeyValuePair<IncomeCategory, IncomeRecord>>())
            .GroupBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Value).ToList());

        var records = new List<KeyValuePair<IncomeCategory, IncomeRecord>>();
        foreach (var record in oldRecords)
        {
            if (record.Value.Count == 2)
            {
                records.Add(new KeyValuePair<IncomeCategory, IncomeRecord>(record.Key, IncomeRecord.TakeWorse(record.Value[0], record.Value[1])));
            }
            else
            {
                records.Add(new KeyValuePair<IncomeCategory, IncomeRecord>(record.Key, IncomeRecord.TakeWorse(record.Value[0], default)));
            }
        }

        var allBonus = Math.Min(left.allBonus, right.allBonus);
        var maintenance = Math.Min(left.maintenance, right.maintenance);
        return new Income(records, allBonus, maintenance);
    }

    public double GetIncome(int fertilityLevel)
    {
        if (this.records == null)
        {
            return 0d;
        }

        var income = 0d;
        foreach (var record in this.records)
        {
            income += (record.Value + new IncomeRecord(0, this.allBonus, 0)).GetIncome(fertilityLevel);
        }

        var result = this.maintenance + income;
        return result;
    }

    public bool Equals(Income other)
    {
        if (this.allBonus != other.allBonus)
        {
            return false;
        }

        if (this.maintenance != other.maintenance)
        {
            return false;
        }

        if (this.records?.Count() != other.records?.Count())
        {
            return false;
        }

        if (this.records != null)
        {
            foreach (var record in this.records)
            {
                if (!record.Value.Equals(other.records.SingleOrDefault(x => x.Key == record.Key).Value))
                {
                    return false;
                }
            }
        }

        return true;
    }
}