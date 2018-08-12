namespace TWBuildingAssistant.Model.Religions
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents an in-game religious influence resulting from a religion.
    /// </summary>
    [Table("ReligionInfluences")]
    public class ReligionInfluence : Effects.Influence
    {
        /// <summary>
        /// Gets or sets the primary key of this <see cref="ReligionInfluence"/> object.
        /// </summary>
        [Key]
        public int ReligionInfluenceId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key of this <see cref="ReligionInfluence"/> object refering to a corresponding <see cref="ReligionEffect"/>.
        /// </summary>
        [ForeignKey("Effect")]
        public int ReligionEffectId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ReligionEffect"/> object that contains this <see cref="ReligionInfluence"/> object.
        /// </summary>
        public ReligionEffect Effect { get; set; }

        /// <summary>
        /// Gets or sets the foreign key of this <see cref="ReligionInfluence"/> object refering to a <see cref="Religions.Religion"/> this influence benefits.
        /// </summary>
        [ForeignKey("Religion")]
        public int? ReligionId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Religions.Religion"/> this <see cref="ReligionInfluence"/> benefits. Null means that this influence always benefits the state religion.
        /// </summary>
        public new Religion Religion
        {
            get => base.Religion as Religion;
            set => base.Religion = value;
        }
    }
}