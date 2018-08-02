namespace TWBuildingAssistant.Data
{
    using System.Data.Entity;

    /// <summary>
    /// Provides acces to world's data containeded within SQLite database.
    /// </summary>
    public class DataModel : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataModel"/> class.
        /// </summary>
        public DataModel()
        : base("name=DataModel")
        {
        }

        /// <summary>
        /// Gets or sets the table containing resources.
        /// </summary>
        public virtual DbSet<Model.Resources.Resource> Resources { get; set; }

        public virtual DbSet<Model.Religions.Religion> Religions { get; set; }

        public virtual DbSet<Model.Religions.ReligionEffect> ReligionEffects { get; set; }

        public virtual DbSet<Model.Religions.ReligionBonus> ReligionBonuses { get; set; }

        public virtual DbSet<Model.Religions.ReligionInfluence> ReligionInfluences { get; set; }
    }
}
