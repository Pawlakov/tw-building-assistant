namespace TWBuildingAssistant.Model.Religions
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ReligionBonuses")]
    public class ReligionBonus : Effects.Bonus
    {
        [Key]
        public int ReligionBonusId { get; set; }

        [ForeignKey("Effect")]
        public int ReligionEffectId { get; set; }

        public ReligionEffect Effect { get; set; }
    }
}