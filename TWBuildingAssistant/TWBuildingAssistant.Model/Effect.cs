namespace TWBuildingAssistant.Model
{
    using System;

    public struct Effect : IEquatable<Effect>
    {
        private readonly int publicOrder;

        private readonly int regularFood;

        private readonly int fertilityDependentFood;

        private readonly int provincialSanitation;

        private readonly int researchRate;

        private readonly int growth;

        private readonly int fertility;

        private readonly int religiousOsmosis;

        private readonly int regionalSanitation;

        private readonly Income income;

        private readonly Influence influence;

        public Effect(int publicOrder = 0, int regularFood = 0, int fertilityDependentFood = 0, int provincialSanitation = 0, int researchRate = 0, int growth = 0, int fertility = 0, int religiousOsmosis = 0, int regionalSanitation = 0, Income income = default, Influence influence = default)
        {
            this.publicOrder = publicOrder;
            this.regularFood = regularFood;
            this.fertilityDependentFood = fertilityDependentFood;
            this.provincialSanitation = provincialSanitation;
            this.researchRate = researchRate;
            this.growth = growth;
            this.fertility = fertility;
            this.religiousOsmosis = religiousOsmosis;
            this.regionalSanitation = regionalSanitation;
            this.income = income;
            this.influence = influence;
        }

        public static Effect operator +(Effect left, Effect right)
        {
            var result = new Effect(
                left.publicOrder + right.publicOrder,
                left.regularFood + right.regularFood,
                left.fertilityDependentFood + right.fertilityDependentFood,
                left.provincialSanitation + right.provincialSanitation,
                left.researchRate + right.researchRate,
                left.growth + right.growth,
                left.fertility + right.fertility,
                left.religiousOsmosis + right.religiousOsmosis,
                left.regionalSanitation + right.regionalSanitation,
                left.income + right.income,
                left.influence + right.influence);

            return result;
        }

        public bool Equals(Effect other)
        {
            return this.publicOrder == other.publicOrder &&
                this.regularFood == other.regularFood &&
                this.fertilityDependentFood == other.fertilityDependentFood &&
                this.provincialSanitation == other.provincialSanitation &&
                this.researchRate == other.researchRate &&
                this.growth == other.growth &&
                this.fertility == other.fertility &&
                this.religiousOsmosis == other.religiousOsmosis &&
                this.regionalSanitation == other.regionalSanitation &&
                this.income.Equals(other.income) &&
                this.influence.Equals(other.influence);
        }
    }
}