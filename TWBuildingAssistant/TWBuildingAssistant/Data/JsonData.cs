namespace TWBuildingAssistant.Data
{
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using TWBuildingAssistant.Model;
    using TWBuildingAssistant.Model.Climate;
    using TWBuildingAssistant.Model.Map;
    using TWBuildingAssistant.Model.Religions;
    using TWBuildingAssistant.Model.Resources;
    using TWBuildingAssistant.Model.Weather;

    public class JsonData : ISource
    {
        private const string JsonFileName = @"Json\twa_data.json";

        public JsonData()
        {
            var file = new FileInfo(JsonFileName);
            using (var reader = file.OpenText())
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var root = JObject.Load(jsonReader);
                    var serializer = new JsonSerializer
                                         {
                                             MissingMemberHandling = MissingMemberHandling.Error
                                         };

                    this.Climates = root.Property(nameof(this.Climates)).Value.ToObject<Climate[]>(serializer);
                    this.Religions = root.Property(nameof(this.Religions)).Value.ToObject<Religion[]>(serializer);
                    this.Resources = root.Property(nameof(this.Resources)).Value.ToObject<Resource[]>(serializer);
                    this.Weathers = root.Property(nameof(this.Weathers)).Value.ToObject<Weather[]>(serializer);
                    this.Provinces = root.Property(nameof(this.Provinces)).Value.ToObject<Province[]>(serializer);
                }
            }
        }

        public IEnumerable<IClimate> Climates { get; set; }

        public IEnumerable<IReligion> Religions { get; set; }
        
        public IEnumerable<IResource> Resources { get; set; }
        
        public IEnumerable<IWeather> Weathers { get; set; }

        public IEnumerable<IProvince> Provinces { get; set; }
    }
}
