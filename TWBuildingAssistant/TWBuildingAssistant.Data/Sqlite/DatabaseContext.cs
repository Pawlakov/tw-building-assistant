namespace TWBuildingAssistant.Data.Sqlite
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using TWBuildingAssistant.Data.Sqlite.Entities;

    public class DatabaseContext : DbContext
    {
        private const string File = @"Data Source=Sqlite\twa_data.db";

        public DbSet<Bonus> Bonuses { get; set; }

        public DbSet<BuildingBranch> BuildingBranches { get; set; }

        public DbSet<BuildingBranchUse> BuildingBranchUses { get; set; }

        public DbSet<BuildingLevel> BuildingLevels { get; set; }

        public DbSet<BuildingLevelLock> BuildingLevelLocks { get; set; }

        public DbSet<Climate> Climates { get; set; }

        public DbSet<Faction> Factions { get; set; }

        public DbSet<Influence> Influences { get; set; }

        public DbSet<Province> Provinces { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Effect> Effects { get; set; }

        public DbSet<Religion> Religions { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<TechnologyLevel> TechnologyLevels { get; set; }

        public DbSet<Weather> Weathers { get; set; }

        public DbSet<WeatherEffect> WeatherEffects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(File);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuildingBranchUse>()
                .HasKey(x => new { x.FactionId, x.BuildingBranchId });

            modelBuilder.Entity<BuildingLevelLock>()
                .HasKey(x => new { x.TechnologyLevelId, x.BuildingLevelId });

            modelBuilder.Entity<WeatherEffect>()
                .HasKey(x => new { x.ClimateId, x.WeatherId });
        }
    }
}