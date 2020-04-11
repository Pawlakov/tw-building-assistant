namespace TWBuildingAssistant.Data.Sqlite
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using TWBuildingAssistant.Data.Sqlite.Model;

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

        public DbSet<ProvincialEffect> ProvincialEffects { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<RegionalEffect> RegionalEffects { get; set; }

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
            modelBuilder.Entity<Bonus>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Bonus>()
                .HasOne<ProvincialEffect>()
                .WithMany()
                .HasForeignKey(x => x.ProvincialEffectId);
            modelBuilder.Entity<Bonus>()
                .HasOne<RegionalEffect>()
                .WithMany()
                .HasForeignKey(x => x.RegionalEffectId);

            modelBuilder.Entity<BuildingBranch>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<BuildingBranch>()
                .Property(x => x.Name)
                .IsRequired();
            modelBuilder.Entity<BuildingBranch>()
                .HasOne<BuildingLevel>()
                .WithOne()
                .HasForeignKey<BuildingBranch>(x => x.RootBuildingLevelId);
            modelBuilder.Entity<BuildingBranch>()
                .HasOne<Religion>()
                .WithMany()
                .HasForeignKey(x => x.ReligionId);
            modelBuilder.Entity<BuildingBranch>()
                .HasOne<Resource>()
                .WithMany()
                .HasForeignKey(x => x.ResourceId);

            modelBuilder.Entity<BuildingBranchUse>()
                .HasKey(x => new { x.FactionId, x.BuildingBranchId });
            modelBuilder.Entity<BuildingBranchUse>()
                .HasOne<BuildingBranch>()
                .WithMany()
                .HasForeignKey(x => x.BuildingBranchId);
            modelBuilder.Entity<BuildingBranchUse>()
                .HasOne<Faction>()
                .WithMany()
                .HasForeignKey(x => x.FactionId);

            modelBuilder.Entity<BuildingLevel>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<BuildingLevel>()
                .Property(x => x.Name)
                .IsRequired();
            modelBuilder.Entity<BuildingLevel>()
                .HasOne<BuildingLevel>()
                .WithMany()
                .HasForeignKey(x => x.ParentBuildingLevelId);
            modelBuilder.Entity<BuildingLevel>()
                .HasOne<RegionalEffect>()
                .WithOne()
                .HasForeignKey<BuildingLevel>(x => x.RegionalEffectId);

            modelBuilder.Entity<BuildingLevelLock>()
                .HasKey(x => new { x.TechnologyLevelId, x.BuildingLevelId });
            modelBuilder.Entity<BuildingLevelLock>()
                .HasOne<TechnologyLevel>()
                .WithMany()
                .HasForeignKey(x => x.TechnologyLevelId);
            modelBuilder.Entity<BuildingLevelLock>()
                .HasOne<BuildingLevel>()
                .WithMany()
                .HasForeignKey(x => x.BuildingLevelId);

            modelBuilder.Entity<Climate>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Climate>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder.Entity<Faction>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Faction>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder.Entity<Influence>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Influence>()
                .HasOne<Religion>()
                .WithMany()
                .HasForeignKey(x => x.ReligionId);
            modelBuilder.Entity<Influence>()
                .HasOne<ProvincialEffect>()
                .WithMany()
                .HasForeignKey(x => x.ProvincialEffectId);
            modelBuilder.Entity<Influence>()
                .HasOne<RegionalEffect>()
                .WithMany()
                .HasForeignKey(x => x.RegionalEffectId);

            modelBuilder.Entity<Province>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Province>()
                .Property(x => x.Name)
                .IsRequired();
            modelBuilder.Entity<Province>()
                .HasOne<Climate>()
                .WithMany()
                .HasForeignKey(x => x.ClimateId);
            modelBuilder.Entity<Province>()
                .HasOne<ProvincialEffect>()
                .WithOne()
                .HasForeignKey<Province>(x => x.ProvincialEffectId);

            modelBuilder.Entity<ProvincialEffect>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Region>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Region>()
                .Property(x => x.Name)
                .IsRequired();
            modelBuilder.Entity<Region>()
                .HasOne<Province>()
                .WithMany()
                .HasForeignKey(x => x.ProvinceId);
            modelBuilder.Entity<Region>()
                .HasOne<Resource>()
                .WithMany()
                .HasForeignKey(x => x.ResourceId);

            modelBuilder.Entity<RegionalEffect>()
                .HasBaseType((Type)null);
            modelBuilder.Entity<RegionalEffect>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Religion>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Religion>()
                .Property(x => x.Name)
                .IsRequired();
            modelBuilder.Entity<Religion>()
                .HasOne<ProvincialEffect>()
                .WithOne()
                .HasForeignKey<Religion>(x => x.ProvincialEffectId);

            modelBuilder.Entity<Resource>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Resource>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder.Entity<TechnologyLevel>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<TechnologyLevel>()
                .HasOne<Faction>()
                .WithMany()
                .HasForeignKey(x => x.FactionId);
            modelBuilder.Entity<TechnologyLevel>()
                .HasOne<ProvincialEffect>()
                .WithOne()
                .HasForeignKey<TechnologyLevel>(x => x.AntilegacyProvincialEffectId);
            modelBuilder.Entity<TechnologyLevel>()
                .HasOne<ProvincialEffect>()
                .WithOne()
                .HasForeignKey<TechnologyLevel>(x => x.UniversalProvincialEffectId);

            modelBuilder.Entity<Weather>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Weather>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder.Entity<WeatherEffect>()
                .HasKey(x => new { x.ClimateId, x.WeatherId });
            modelBuilder.Entity<WeatherEffect>()
                .HasOne<Climate>()
                .WithMany()
                .HasForeignKey(x => x.ClimateId);
            modelBuilder.Entity<WeatherEffect>()
                .HasOne<Weather>()
                .WithMany()
                .HasForeignKey(x => x.WeatherId);
            modelBuilder.Entity<WeatherEffect>()
                .HasOne<ProvincialEffect>()
                .WithOne()
                .HasForeignKey<WeatherEffect>(x => x.ProvincialEffectId);
        }
    }
}