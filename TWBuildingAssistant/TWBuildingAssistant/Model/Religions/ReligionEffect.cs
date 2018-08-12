namespace TWBuildingAssistant.Model.Religions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents a set of effects acting on an entire province resulting from a religion.
    /// </summary>
    [Table("ReligionEffects")]
    public class ReligionEffect : Effects.ProvincionalEffect
    {
        /// <summary>
        /// Gets or sets the primary key of this <see cref="ReligionEffect"/> object which is simultaneously also the foreign key refering to a corresponding <see cref="Religions.Religion"/>.
        /// </summary>
        [Key]
        [ForeignKey("Religion")]
        public int ReligionEffectId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Religions.Religion"/> this <see cref="ReligionEffect"/> object belongs to.
        /// </summary>
        public Religion Religion { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="ReligionBonus"/> objects belonging to this <see cref="ReligionEffect"/> object.
        /// </summary>
        public ICollection<ReligionBonus> BonusesNavigation { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="ReligionInfluence"/> objects belonging to this <see cref="ReligionEffect"/> object.
        /// </summary>
        public ICollection<ReligionInfluence> InfluencesNavigation { get; set; }

        /// <summary>
        /// Returns a simplified equivalent of this object.
        /// </summary>
        /// <returns>
        /// The <see cref="Effects.IProvincionalEffect"/> which is a simplified equivalent of this object.
        /// </returns>
        public Effects.IProvincionalEffect Simplify()
        {
            return this.Aggregate(
            new Effects.ProvincionalEffect
            {
            Bonuses = from Effects.IBonus bonus in this.BonusesNavigation select bonus,
            Influences = from Effects.IInfluence influence in this.InfluencesNavigation select influence
            });
        }
    }
}