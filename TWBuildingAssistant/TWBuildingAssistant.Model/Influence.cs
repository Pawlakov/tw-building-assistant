namespace TWBuildingAssistant.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public struct Influence : IEquatable<Influence>
    {
        private readonly IDictionary<Religion, int> records;

        public Influence(Religion religion, int value)
        {
            if (value <= 0)
            {
                throw new DomainRuleViolationException("Nonpositive influence.");
            }

            this.records = new Dictionary<Religion, int>
            {
                { religion, value },
            };
        }

        private Influence(IDictionary<Religion, int> records)
        {
            this.records = records.ToDictionary(x => x.Key, x => x.Value);
        }

        public static Influence operator +(Influence left, Influence right)
        {
            var records = left.records.ToDictionary(x => x.Key, x => x.Value);
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

            return new Influence(records);
        }

        public int PublicOrder(Religion stateReligion)
        {
            var percentage = this.Percentage(stateReligion);
            var result = -(int)Math.Floor((750 - (percentage * 7)) * 0.01);
            return result;
        }

        public double Percentage(Religion stateReligion)
        {
            var state = 0;
            var all = 0;
            foreach (var record in this.records)
            {
                all += record.Value;
                if (record.Key == stateReligion || record.Key == null)
                {
                    state += record.Value;
                }
            }

            if (all == 0)
            {
                return 100;
            }

            var percentage = 100 * (state / (double)all);
            return percentage;
        }

        public bool Equals(Influence other)
        {
            if (this.records.Count != other.records.Count)
            {
                return false;
            }

            foreach (var record in this.records)
            {
                if (!other.records.ContainsKey(record.Key) || !record.Value.Equals(other.records[record.Key]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}