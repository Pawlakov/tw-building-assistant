namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    public class JsonSource<TModel, TJsonModel> : IRepository<TModel>
        where TJsonModel : class, TModel
    {
        public JsonSource(string filePath)
        {
            var baseLocation = typeof(ResourceRepository).Assembly.Location;
            var assemblyFile = new FileInfo(baseLocation);
            var file = new FileInfo(assemblyFile.DirectoryName + "\\" + filePath);
            using (var reader = file.OpenText())
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var serializer = new JsonSerializer
                    {
                        MissingMemberHandling = MissingMemberHandling.Error,
                    };

                    this.DataSet = serializer
                        .Deserialize<IEnumerable<TJsonModel>>(jsonReader)
                        .Cast<TModel>()
                        .ToList();
                }
            }
        }

        public IList<TModel> DataSet { get; }
    }
}
