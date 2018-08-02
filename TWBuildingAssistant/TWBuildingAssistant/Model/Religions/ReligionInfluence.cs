namespace TWBuildingAssistant.Model.Religions
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ReligionInfluences")]
    public class ReligionInfluence : Effects.Influence
    {
        [Key]
        public int ReligionInfluenceId { get; set; }

        [ForeignKey("Effect")]
        public int ReligionEffectId { get; set; }

        public ReligionEffect Effect { get; set; }

        [ForeignKey("Religion")]
        public int? ReligionId { get; set; }

        public new Religion Religion
        {
            get => base.Religion as Religion;
            set => base.Religion = value;
        }
    }
}