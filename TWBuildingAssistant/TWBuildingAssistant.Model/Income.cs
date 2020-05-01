namespace TWBuildingAssistant.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EnumsNET;
    using TWBuildingAssistant.Data.Model;

    public struct Income : IEquatable<Income>
    {
        private readonly IDictionary<IncomeCategory, IncomeRecord> records;

        private readonly int allBonus;

        public Income(int value, IncomeCategory? category, BonusType type)
        {
            if (value == 0)
            {
                throw new DomainRuleViolationException("'0' income.");
            }

            if (value < 0 && category != IncomeCategory.Maintenance)
            {
                throw new DomainRuleViolationException("Negative income.");
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

            this.records = new Dictionary<IncomeCategory, IncomeRecord>();
            this.allBonus = 0;
            if (category != null)
            {
                this.records.Add(category.Value, new IncomeRecord(value, type));
            }
            else
            {
                this.allBonus = value;
            }
        }

        private Income(IDictionary<IncomeCategory, IncomeRecord> records, int allBonus)
        {
            this.allBonus = allBonus;
            if (records != null)
            {
                this.records = records.ToDictionary(x => x.Key, x => x.Value);
            }
            else
            {
                this.records = new Dictionary<IncomeCategory, IncomeRecord>();
            }
        }

        public static Income operator +(Income left, Income right)
        {
            var records = new Dictionary<IncomeCategory, IncomeRecord>();
            if (left.records != null)
            {
                records = left.records.ToDictionary(x => x.Key, x => x.Value);
            }

            if (right.records != null)
            {
                foreach (var record in right.records)
                {
                    if (records.ContainsKey(record.Key))
                    {
                        records[record.Key] = records[record.Key] + record.Value;
                    }
                    else
                    {
                        records.Add(record.Key, record.Value);
                    }
                }
            }

            var allBonus = left.allBonus + right.allBonus;
            return new Income(records, allBonus);
        }

        public static Income TakeWorse(Income left, Income right)
        {
            var oldRecords = (left.records?.ToList() ?? new List<KeyValuePair<IncomeCategory, IncomeRecord>>())
                .Concat(right.records?.ToList() ?? new List<KeyValuePair<IncomeCategory, IncomeRecord>>())
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Value).ToList());

            var records = new Dictionary<IncomeCategory, IncomeRecord>();
            foreach (var record in oldRecords)
            {
                if (record.Value.Count == 2)
                {
                    records.Add(record.Key, IncomeRecord.TakeWorse(record.Value[0], record.Value[1]));
                }
                else
                {
                    records.Add(record.Key, IncomeRecord.TakeWorse(record.Value[0], default));
                }
            }

            var allBonus = Math.Min(left.allBonus, right.allBonus);
            return new Income(records, allBonus);
        }

        public double GetIncome(int fertilityLevel)
        {
            if (this.records == null)
            {
                return 0d;
            }

            var maintenance = 0d;
            var income = 0d;
            foreach (var key in this.records.Keys)
            {
                if (key == IncomeCategory.Maintenance)
                {
                    maintenance += this.records[key].GetIncome(fertilityLevel);
                }
                else
                {
                    income += this.records[key].GetIncome(fertilityLevel);
                }
            }

            var result = maintenance + (income * (1 + this.allBonus));
            return result;
        }

        public bool Equals(Income other)
        {
            if (this.allBonus != other.allBonus)
            {
                return false;
            }

            if (this.records?.Count != other.records?.Count)
            {
                return false;
            }

            if (this.records != null)
            {
                foreach (var record in this.records)
                {
                    if (!other.records.ContainsKey(record.Key) || !record.Value.Equals(other.records[record.Key]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}