namespace TWBuildingAssistant.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using TWBuildingAssistant.Model;
    using TWBuildingAssistant.Model.Climate;
    using TWBuildingAssistant.Model.Religions;
    using TWBuildingAssistant.Model.Resources;
    using TWBuildingAssistant.Model.Weather;

    public partial class JsonData : IReligionsSource, IResourcesSource, IClimateSource, IWeatherSource
    {
        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(JsonConcreteConverter<Religion[]>))]
        public IEnumerable<IReligion> Religions { get; set; }

        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(JsonConcreteConverter<Resource[]>))]
        public IEnumerable<IResource> Resources { get; set; }

        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(JsonConcreteConverter<Climate[]>))]
        public IEnumerable<IClimate> Climates { get; set; }

        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(JsonConcreteConverter<Weather[]>))]
        public IEnumerable<IWeather> Weathers { get; set; }
    }

    public partial class JsonData
    {
        private static string jsonFileName = @"Json\twa_data.json";

        private static JsonData jsonData;

        private JsonData()
        {
        }

        public static JsonData GetData()
        {
            return jsonData ?? (jsonData = CreateFromJson());
        }

        private static JsonData CreateFromJson()
        {
            var file = new FileInfo(jsonFileName);
            using (var reader = file.OpenText())
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var serializer = new JsonSerializer
                                         {
                                             MissingMemberHandling = MissingMemberHandling.Error
                                         };
                    var result = serializer.Deserialize<JsonData>(jsonReader);
                    return result;
                }
            }

        }
    }
}
