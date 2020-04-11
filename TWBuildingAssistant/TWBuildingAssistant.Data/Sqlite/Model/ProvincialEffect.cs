namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class ProvincialEffect : IProvincialEffect
    {
        public ProvincialEffect()
        {
        }

        public ProvincialEffect(IProvincialEffect source)
        {
            this.Id = source.Id;
            this.PublicOrder = source.PublicOrder;
            this.RegularFood = source.RegularFood;
            this.FertilityDependentFood = source.FertilityDependentFood;
            this.ProvincialSanitation = source.ProvincialSanitation;
            this.ResearchRate = source.ResearchRate;
            this.Growth = source.Growth;
            this.Fertility = source.Fertility;
            this.ReligiousOsmosis = source.ReligiousOsmosis;
        }

        public int Id { get; set; }

        public int PublicOrder { get; set; }

        public int RegularFood { get; set; }

        public int FertilityDependentFood { get; set; }

        public int ProvincialSanitation { get; set; }

        public int ResearchRate { get; set; }

        public int Growth { get; set; }

        public int Fertility { get; set; }

        public int ReligiousOsmosis { get; set; }
    }
}