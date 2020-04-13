namespace TWBuildingAssistant.Model
{
    using System;

    public struct Effect : IEquatable<Effect>
    {
        public Effect(int publicOrder = 0, int regularFood = 0, int fertilityDependentFood = 0, int provincialSanitation = 0, int researchRate = 0, int growth = 0, int fertility = 0, int religiousOsmosis = 0, int regionalSanitation = 0)
        {
            this.PublicOrder = publicOrder;
            this.RegularFood = regularFood;
            this.FertilityDependentFood = fertilityDependentFood;
            this.ProvincialSanitation = provincialSanitation;
            this.ResearchRate = researchRate;
            this.Growth = growth;
            this.Fertility = fertility;
            this.ReligiousOsmosis = religiousOsmosis;
            this.RegionalSanitation = regionalSanitation;
        }

        public int PublicOrder { get; }

        public int RegularFood { get; }

        public int FertilityDependentFood { get; }

        public int ProvincialSanitation { get; }

        public int ResearchRate { get; }

        public int Growth { get; }

        public int Fertility { get; }

        public int ReligiousOsmosis { get; }

        public int RegionalSanitation { get; }

        public static Effect operator +(Effect left, Effect right)
        {
            var result = new Effect(
                left.PublicOrder + right.PublicOrder,
                left.RegularFood + right.RegularFood,
                left.FertilityDependentFood + right.FertilityDependentFood,
                left.ProvincialSanitation + right.ProvincialSanitation,
                left.ResearchRate + right.ResearchRate,
                left.Growth + right.Growth,
                left.Fertility + right.Fertility,
                left.ReligiousOsmosis + right.ReligiousOsmosis,
                left.RegionalSanitation + right.RegionalSanitation);

            return result;
        }

        public bool Equals(Effect other)
        {
            return this.PublicOrder == other.PublicOrder &&
                this.RegularFood == other.RegularFood &&
                this.FertilityDependentFood == other.FertilityDependentFood &&
                this.ProvincialSanitation == other.ProvincialSanitation &&
                this.ResearchRate == other.ResearchRate &&
                this.Growth == other.Growth &&
                this.Fertility == other.Fertility &&
                this.ReligiousOsmosis == other.ReligiousOsmosis &&
                this.RegionalSanitation == other.RegionalSanitation;
        }
    }
}