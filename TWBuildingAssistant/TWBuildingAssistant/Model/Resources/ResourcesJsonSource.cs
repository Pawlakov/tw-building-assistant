namespace TWBuildingAssistant.Model.Resources
{
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json;

    public class ResourcesJsonSource : IResourcesSource
    {
        private const string SourceFile = @"Json\twa_resources.json";

        public IEnumerable<IResource> GetResources()
        {
            var settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error
            };
            var json = File.ReadAllText(SourceFile);
            return JsonConvert.DeserializeObject<Resource[]>(json, settings);
        }
    }
}