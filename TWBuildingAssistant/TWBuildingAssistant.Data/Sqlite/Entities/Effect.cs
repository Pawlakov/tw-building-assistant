namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    public class Effect
    {
        public int Id { get; }

        public int PublicOrder { get; }

        public int RegularFood { get; }

        public int FertilityDependentFood { get; }

        public int ProvincialSanitation { get; }

        public int ResearchRate { get; }

        public int Growth { get; }

        public int Fertility { get; }

        public int ReligiousOsmosis { get; }

        public int RegionalSanitation { get; set; }
    }
}