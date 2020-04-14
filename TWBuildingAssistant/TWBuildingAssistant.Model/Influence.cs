namespace TWBuildingAssistant.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public struct Influence : IEquatable<Influence>
    {
        private readonly IDictionary<object, int> records;

        public Influence(IEnumerable<object> religions, object religion, int value)
        {
            this.records = new Dictionary<object, int>();
            foreach (var member in religions)
            {
                if (member == religion)
                {
                    this.records.Add(member, value);
                }
                else
                {
                    this.records.Add(member, 0);
                }
            }
        }

        private Influence(IDictionary<object, int> records)
        {
            this.records = records.ToDictionary(x => x.Key, x => x.Value);
        }

        public static Influence operator +(Influence left, Influence right)
        {
            var records = new Dictionary<object, int>();
            foreach (var member in left.records.Keys)
            {
                records.Add(member, left.records[member] + right.records[member]);
            }

            return new Influence(records);
        }

        public int PublicOrder(object stateReligion)
        {
            var percentage = this.Percentage(stateReligion);
            var result = -(int)Math.Floor((750 - (percentage * 7)) * 0.01);
            return result;
        }

        public double Percentage(object stateReligion)
        {
            var state = this.records[stateReligion];
            var all = this.records.Sum(x => x.Value);

            if (all == 0)
            {
                return 100;
            }

            var percentage = 100 * (state / (double)all);
            return percentage;
        }

        public bool Equals(Influence other)
        {
            foreach (var member in this.records.Keys)
            {
                if (!this.records[member].Equals(other.records[member]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}