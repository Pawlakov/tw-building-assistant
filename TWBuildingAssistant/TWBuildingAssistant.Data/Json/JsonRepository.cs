namespace TWBuildingAssistant.Data.Json
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    public class JsonRepository<TModel> : IRepository<TModel>
    {
        public JsonRepository()
        {
            var type = typeof(TModel);
            var assembly = type.Assembly;
            var types = assembly.GetTypes();
            var namespaceTypes = types.Where(x => x.Namespace == "TWBuildingAssistant.Data.Json.Model");
            var concrete = namespaceTypes.Where(x => type.IsAssignableFrom(x)).OrderBy(x => x.GetInterfaces().Count()).First();
            var concreateEnumerable = typeof(List<>).MakeGenericType(concrete);
            var assemblyFile = new FileInfo(assembly.Location);
            var file = new FileInfo(assemblyFile.DirectoryName + @"\Json\twa_data_" + concrete.Name + @".json");
            using (var reader = file.OpenText())
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var serializer = new JsonSerializer
                    {
                        MissingMemberHandling = MissingMemberHandling.Error,
                    };

                    var list = (IEnumerable)serializer.Deserialize(jsonReader, concreateEnumerable);
                    this.DataSet = list.Cast<TModel>();
                }
            }
        }

        public IEnumerable<TModel> DataSet { get; }
    }
}