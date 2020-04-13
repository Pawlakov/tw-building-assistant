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

        public Income(int value, IncomeCategory category, BonusType type)
        {
            this.records = new Dictionary<IncomeCategory, IncomeRecord>();
            foreach (var member in Enums.GetMembers<IncomeCategory>())
            {
                if (member.Value == category)
                {
                    this.records.Add(member.Value, new IncomeRecord(value, type));
                }
                else
                {
                    this.records.Add(member.Value, new IncomeRecord(0, 0, 0));
                }
            }
        }

        private Income(IDictionary<IncomeCategory, IncomeRecord> records)
        {
            this.records = records.ToDictionary(x => x.Key, x => x.Value);
        }

        public static Income operator +(Income left, Income right)
        {
            var records = new Dictionary<IncomeCategory, IncomeRecord>();
            foreach (var member in Enums.GetMembers<IncomeCategory>())
            {
                records.Add(member.Value, left.records[member.Value] + right.records[member.Value]);
            }

            return new Income(records);
        }

        public double GetIncome(int fertilityLevel)
        {
            var categoriesAffectedByAll = Enums.GetMembers<IncomeCategory>().Select(x => x.Value).Where(x => x != IncomeCategory.All && x != IncomeCategory.Maintenance);
            foreach (var member in categoriesAffectedByAll)
            {
                this.records[member] = this.records[member] + this.records[IncomeCategory.All];
            }

            var result = this.records.Sum(x => x.Value.GetIncome(fertilityLevel));
            return result;
        }

        public bool Equals(Income other)
        {
            foreach (var member in Enums.GetMembers<IncomeCategory>())
            {
                if (!this.records[member.Value].Equals(other.records[member.Value]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}