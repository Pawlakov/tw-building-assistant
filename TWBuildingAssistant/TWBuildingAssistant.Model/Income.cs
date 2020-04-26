﻿namespace TWBuildingAssistant.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EnumsNET;
    using TWBuildingAssistant.Data.Model;

    public struct Income : IEquatable<Income>
    {
        private readonly IDictionary<IncomeCategory, IncomeRecord> records;

        public Income(int value, IncomeCategory category, BonusType type)
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

            if (category == IncomeCategory.All && type != BonusType.Percentage)
            {
                throw new DomainRuleViolationException("Invalid 'All' income.");
            }

            if (type == BonusType.FertilityDependent && category != IncomeCategory.Husbandry && category != IncomeCategory.Agriculture)
            {
                throw new DomainRuleViolationException("Invalid fertility-based income.");
            }

            this.records = new Dictionary<IncomeCategory, IncomeRecord>
            {
                { category, new IncomeRecord(value, type) },
            };
        }

        private Income(IDictionary<IncomeCategory, IncomeRecord> records)
        {
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

            return new Income(records);
        }

        public double GetIncome(int fertilityLevel)
        {
            if (this.records == null)
            {
                return 0d;
            }

            if (this.records.ContainsKey(IncomeCategory.All))
            {
                foreach (var key in this.records.Keys.Where(x => x != IncomeCategory.All && x != IncomeCategory.Maintenance))
                {
                    this.records[key] = this.records[key] + this.records[IncomeCategory.All];
                }
            }

            var result = this.records.Sum(x => x.Value.GetIncome(fertilityLevel));
            return result;
        }

        public bool Equals(Income other)
        {
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