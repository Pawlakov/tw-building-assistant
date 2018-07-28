namespace TWBuildingAssistant.Data
{
    using System.Data.Entity;

    public class DataModel : DbContext
    {
        public DataModel()
        : base("name=DataModel")
        {
        }

        public virtual DbSet<Model.Resources.Resource> Resources { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
