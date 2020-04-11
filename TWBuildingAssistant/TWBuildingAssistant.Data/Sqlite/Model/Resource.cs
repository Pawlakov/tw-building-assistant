namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class Resource : IResource
    {
        public Resource()
        {
        }

        public Resource(IResource source)
        {
            this.Id = source.Id;
            this.Name = source.Name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}