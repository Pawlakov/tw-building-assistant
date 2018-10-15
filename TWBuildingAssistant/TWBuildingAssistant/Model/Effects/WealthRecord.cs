namespace TWBuildingAssistant.Model.Effects
{
    public class WealthRecord
    {
        private readonly int[] values = new int[3];
        
        public int this[BonusType index]
        {
            get => this.values[(int)index];
            set => this.values[(int)index] = value;
        }
        
        public double Calculate(int fertility)
        {
            return (this[BonusType.Simple] + (this[BonusType.FertilityDependent] * fertility)) * ((100 + this[BonusType.Percentage]) * 0.01);
        }

        public override string ToString()
        {
            return $"+{this[BonusType.Simple]}, +{this[BonusType.Percentage]}%, +{this[BonusType.FertilityDependent]}/FL";
        }
    }
}