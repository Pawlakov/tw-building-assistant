namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    public class Region
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public RegionType RegionType { get; set; }

        public int? ResourceId { get; set; }

        public bool IsCoastal { get; set; }

        public int SlotsCountOffset { get; set; }

        public int ProvinceId { get; set; }
    }
}