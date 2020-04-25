namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TWBuildingAssistant.Data.Model;

    public class Bonus
    {
        [Key]
        public int Id { get; set; }

        public int Value { get; set; }

        public IncomeCategory Category { get; set; }

        public BonusType Type { get; set; }

        [ForeignKey(nameof(Effect))]
        public int? EffectId { get; set; }

        public Effect Effect { get; set; }
    }
}