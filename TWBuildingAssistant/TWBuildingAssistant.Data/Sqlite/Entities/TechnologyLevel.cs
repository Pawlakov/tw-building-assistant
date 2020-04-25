namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TechnologyLevel
    {
        [Key]
        public int Id { get; set; }

        public int Order { get; set; }

        [ForeignKey(nameof(Faction))]
        public int FactionId { get; set; }

        [ForeignKey(nameof(UniversalEffect))]
        public int? UniversalEffectId { get; set; }

        [ForeignKey(nameof(AntilegacyEffect))]
        public int? AntilegacyEffectId { get; set; }

        public Faction Faction { get; set; }

        public Effect UniversalEffect { get; set; }

        public Effect AntilegacyEffect { get; set; }

        public ICollection<BuildingLevelLock> BuildingLevelLocks { get; set; }
    }
}