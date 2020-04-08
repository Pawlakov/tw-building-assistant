namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class ResourceRepository : IRepository<IResource>
    {
        private const string JsonFileName = @"Json\twa_data_resources.json";

        private readonly JsonSource<IResource, Resource> jsonSource;

        public ResourceRepository()
        {
            this.jsonSource = new JsonSource<IResource, Resource>(JsonFileName);
        }

        public IList<IResource> DataSet => this.jsonSource.DataSet;
    }
}