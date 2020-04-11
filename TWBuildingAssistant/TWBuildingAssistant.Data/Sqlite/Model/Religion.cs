namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class Religion : IReligion
    {
        public Religion()
        {
        }

        public Religion(IReligion source)
        {
            this.Id = source.Id;
            this.Name = source.Name;
            this.ProvincialEffectId = source.ProvincialEffectId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? ProvincialEffectId { get; set; }
    }
}