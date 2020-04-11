namespace TWBuildingAssistant.Data.Sqlite.Model
{
    public class Province
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Fertility { get; set; }

        public int ClimateId { get; set; }

        public int? ProvincialEffectId { get; set; }
    }
}