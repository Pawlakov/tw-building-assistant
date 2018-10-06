namespace TWBuildingAssistant.Data
{
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json;

    using TWBuildingAssistant.Model.Religions;
    using TWBuildingAssistant.Model.Resources;

    public partial class JsonData : IReligionsSource, IResourcesSource
    {
        private const string ReligionsJsonFileName = @"Json\twa_religions.json";

        private const string ResourcesJsonFileName = @"Json\twa_resources.json";

        private IEnumerable<IReligion> religions;

        private IEnumerable<IResource> resources;

        public IEnumerable<IReligion> Religions => this.religions ?? (this.religions = this.GetFromJson<Religion>(ReligionsJsonFileName));

        public IEnumerable<IResource> Resources => this.resources ?? (this.resources = this.GetFromJson<Resource>(ResourcesJsonFileName));

        public IEnumerable<T> GetFromJson<T>(string jsonFileName)
        {
            var settings = new JsonSerializerSettings
                               {
                                   MissingMemberHandling = MissingMemberHandling.Error
                               };
            var json = File.ReadAllText(jsonFileName);
            var result = JsonConvert.DeserializeObject<T[]>(json, settings);
            return result;
        }
    }

    public partial class JsonData
    {
        private static JsonData jsonData;

        private JsonData()
        {
        }

        public static JsonData Data => jsonData ?? (jsonData = new JsonData());
    }
}
