namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class Region : IRegion
    {
        public Region()
        {
        }

        public Region(IRegion source)
        {
            this.Id = source.Id;
            this.Name = source.Name;
            this.IsCity = source.IsCity;
            this.ResourceId = source.ResourceId;
            this.IsCoastal = source.IsCoastal;
            this.SlotsCountOffset = source.SlotsCountOffset;
            this.ProvinceId = source.ProvinceId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsCity { get; set; }

        public int? ResourceId { get; set; }

        public bool IsCoastal { get; set; }

        public int SlotsCountOffset { get; set; }

        public int ProvinceId { get; set; }
    }
}