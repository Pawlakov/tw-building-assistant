namespace TWBuildingAssistant.Model.Religions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    using TWBuildingAssistant.Model.Effects;

    [Table("ReligionEffects")]
    public class ReligionEffect : Effects.ProvincionalEffect
    {
        [Key]
        [ForeignKey("Religion")]
        public int ReligionEffectId { get; set; }

        public Religion Religion { get; set; }

        public ICollection<ReligionBonus> BonusesNavigation
        {
            get;
            set;
        }

        public ICollection<ReligionInfluence> InfluencesNavigation
        {
            get;
            set;
        }
        
        public Effects.IProvincionalEffect Simplify()
        {
            return this.Aggregate(new ProvincionalEffect());
        }
    }
}