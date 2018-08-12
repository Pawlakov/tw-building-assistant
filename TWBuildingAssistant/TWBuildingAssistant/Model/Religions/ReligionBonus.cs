namespace TWBuildingAssistant.Model.Religions
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents an in-game wealth bonus resulting from a religion.
    /// </summary>
    [Table("ReligionBonuses")]
    public class ReligionBonus : Effects.Bonus
    {
        /// <summary>
        /// Gets or sets the primary key of this <see cref="ReligionBonus"/> object.
        /// </summary>
        [Key]
        public int ReligionBonusId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key of this <see cref="ReligionBonus"/> object refering to a corresponding <see cref="ReligionEffect"/>.
        /// </summary>
        [ForeignKey("Effect")]
        public int ReligionEffectId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ReligionEffect"/> object that contains this <see cref="ReligionBonus"/> object.
        /// </summary>
        public ReligionEffect Effect { get; set; }
    }
}