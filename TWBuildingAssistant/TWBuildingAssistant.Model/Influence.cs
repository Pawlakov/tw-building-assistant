namespace TWBuildingAssistant.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public struct Influence : IEquatable<Influence>
    {
        private readonly IEnumerable<KeyValuePair<Religion, int>> records;

        private readonly int state;

        public Influence(Religion religion, int value)
        {
            if (value <= 0)
            {
                throw new DomainRuleViolationException("Nonpositive influence.");
            }

            if (religion != null)
            {
                this.records = new List<KeyValuePair<Religion, int>> { new KeyValuePair<Religion, int>(religion, value) };
                this.state = 0;
            }
            else
            {
                this.records = new List<KeyValuePair<Religion, int>>();
                this.state = value;
            }
        }

        private Influence(IEnumerable<KeyValuePair<Religion, int>> records, int state)
        {
            this.state = state;
            if (records != null)
            {
                this.records = records.ToList();
            }
            else
            {
                this.records = new Dictionary<Religion, int>();
            }
        }

        public static Influence operator +(Influence left, Influence right)
        {
            var records = new List<KeyValuePair<Religion, int>>();
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
                        records[presentRecordIndex] = new KeyValuePair<Religion, int>(record.Key, records[presentRecordIndex].Value + record.Value);
                    }
                    else
                    {
                        records.Add(new KeyValuePair<Religion, int>(record.Key, record.Value));
                    }
                }
            }

            var state = left.state + right.state;
            return new Influence(records, state);
        }

        public static Influence TakeWorse(Influence left, Influence right)
        {
            var oldRecords = (left.records?.ToList() ?? new List<KeyValuePair<Religion, int>>())
                .Concat(right.records?.ToList() ?? new List<KeyValuePair<Religion, int>>())
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Value).ToList());

            var records = new Dictionary<Religion, int>();
            foreach (var record in oldRecords)
            {
                if (record.Value.Count == 2)
                {
                    records.Add(record.Key, Math.Min(record.Value[0], record.Value[1]));
                }
                else
                {
                    records.Add(record.Key, Math.Min(record.Value[0], default));
                }
            }

            var allBonus = Math.Min(left.state, right.state);
            return new Influence(records, allBonus);
        }

        public int PublicOrder(Religion stateReligion)
        {
            var percentage = this.Percentage(stateReligion);
            var result = -(int)Math.Floor((750 - (percentage * 7)) * 0.01);
            return result;
        }

        public double Percentage(Religion stateReligion)
        {
            var state = this.state;
            var all = this.state;
            if (this.records != null)
            {
                foreach (var record in this.records)
                {
                    all += record.Value;
                    if (record.Key == stateReligion || record.Key == null)
                    {
                        state += record.Value;
                    }
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
            if (this.state != other.state)
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
}