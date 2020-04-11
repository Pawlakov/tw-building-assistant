namespace TWBuildingAssistant.Data.Sqlite.Model
{
    public class ProvincialEffect
    {
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