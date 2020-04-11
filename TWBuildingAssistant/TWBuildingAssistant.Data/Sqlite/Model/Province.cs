namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class Province : IProvince
    {
        public Province()
        {
        }

        public Province(IProvince source)
        {
            this.Id = source.Id;
            this.Name = source.Name;
            this.Fertility = source.Fertility;
            this.ClimateId = source.ClimateId;
            this.ProvincialEffectId = source.ProvincialEffectId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Fertility { get; set; }

        public int ClimateId { get; set; }

        public int? ProvincialEffectId { get; set; }
    }
}