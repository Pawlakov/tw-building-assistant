namespace TWBuildingAssistant.Model
{
    using System;
    using TWBuildingAssistant.Data.Model;

    internal struct IncomeRecord : IEquatable<IncomeRecord>
    {
        private readonly int[] values;

        public IncomeRecord(int simple = 0, int percentage = 0, int fertilityDependet = 0)
        {
            this.values = new int[3];
            this.values[(int)BonusType.Simple] = simple;
            this.values[(int)BonusType.Percentage] = percentage;
            this.values[(int)BonusType.FertilityDependent] = fertilityDependet;
        }

        public IncomeRecord(int value, BonusType type)
        {
            this.values = new int[3];
            this.values[(int)type] = value;
        }

        public int this[BonusType type] => this.values[(int)type];

        public static IncomeRecord operator +(IncomeRecord left, IncomeRecord right)
        {
            return new IncomeRecord(
                left.values[(int)BonusType.Simple] + right.values[(int)BonusType.Simple],
                left.values[(int)BonusType.Percentage] + right.values[(int)BonusType.Percentage],
                left.values[(int)BonusType.FertilityDependent] + right.values[(int)BonusType.FertilityDependent]);
        }

        public static IncomeRecord TakeWorse(IncomeRecord left, IncomeRecord right)
        {
            return new IncomeRecord(
                Math.Min(left.values[(int)BonusType.Simple], right.values[(int)BonusType.Simple]),
                Math.Min(left.values[(int)BonusType.Percentage], right.values[(int)BonusType.Percentage]),
                Math.Min(left.values[(int)BonusType.FertilityDependent], right.values[(int)BonusType.FertilityDependent]));
        }

        public double GetIncome(int fertility)
        {
            return (this.values[(int)BonusType.Simple] + (this.values[(int)BonusType.FertilityDependent] * fertility)) * ((100 + this.values[(int)BonusType.Percentage]) * 0.01);
        }

        public bool Equals(IncomeRecord other)
        {
            for (var i = 0; i < 3; ++i)
            {
                if (this.values[i] != other.values[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}