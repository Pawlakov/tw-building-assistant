namespace TWBuildingAssistant.Model.Effects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class ProvincionalEffect : IProvincionalEffect
    {
        public ProvincionalEffect()
        {
            this.Bonuses = new List<IBonus>();
            this.Influences = new List<IInfluence>();
        }

        [Column]
        public int? PublicOrder { get; set; }

        [Column]
        public int? RegularFood { get; set; }

        [Column]
        public int? FertilityDependentFood { get; set; }

        public int Food(int fertility)
        {
            return (this.RegularFood ?? 0) + ((this.FertilityDependentFood ?? 0) * fertility);
        }

        [Column]
        public int? ProvincionalSanitation { get; set; }

        [Column]
        public int? ResearchRate { get; set; }

        [Column]
        public int? Growth { get; set; }

        [Column]
        public int? Fertility { get; set; }

        [Column]
        public int? ReligiousOsmosis { get; set; }

        [NotMapped]
        public IEnumerable<IBonus> Bonuses
        {
            get;
            set;
        }

        [NotMapped]
        public IEnumerable<IInfluence> Influences
        {
            get;
            set;
        }

        public IProvincionalEffect Aggregate(IProvincionalEffect other)
        {
            if (other == null)
            {
                other = new ProvincionalEffect();
            }

            return new ProvincionalEffect()
                   {
                   PublicOrder = (this.PublicOrder ?? 0) + (other.PublicOrder ?? 0),
                   RegularFood = (this.RegularFood ?? 0) + (other.RegularFood ?? 0),
                   FertilityDependentFood = (this.FertilityDependentFood ?? 0) + (other.FertilityDependentFood ?? 0),
                   ProvincionalSanitation = (this.ProvincionalSanitation ?? 0) + (other.ProvincionalSanitation ?? 0),
                   ResearchRate = (this.ResearchRate ?? 0) + (other.ResearchRate ?? 0),
                   Growth = (this.Growth ?? 0) + (other.Growth ?? 0),
                   Fertility = (this.Fertility ?? 0) + (other.Fertility ?? 0),
                   ReligiousOsmosis = (this.ReligiousOsmosis ?? 0) + (other.ReligiousOsmosis ?? 0),
                   Bonuses = this.Bonuses.Concat(other.Bonuses).ToList(),
                   Influences = this.Influences.Concat(other.Influences).ToList()
                   };
        }
    }
}