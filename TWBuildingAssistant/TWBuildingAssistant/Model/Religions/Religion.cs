namespace TWBuildingAssistant.Model.Religions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents one of in-game religions.
    /// </summary>
    [Table("Religions")]
    public class Religion : IReligion
    {
        /// <summary>
        /// The state religion tracker that notifies notifies about changes of the state religion.
        /// </summary>
        private IStateReligionTracker stateReligionTracker;

        /// <summary>
        /// Indicates whether this religion is the state religion. Updated using <see cref="IStateReligionTracker"/>.
        /// </summary>
        private bool isState;

        /// <summary>
        /// The <see cref="Effects.IProvincionalEffect"/> taken into account when this <see cref="Religion"/> is the state religion.
        /// </summary>
        private Effects.IProvincionalEffect effect;

        /// <summary>
        /// Gets or sets the primary key of this <see cref="Religion"/> object.
        /// </summary>
        [Key]
        public int ReligionId { get; set; }

        /// <summary>
        /// Gets or sets the name of this <see cref="Religion"/>
        /// </summary>
        [Column]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ReligionEffect"/> taken into account when this <see cref="Religion"/> is the state religion. This property is used in ORM navigation.
        /// </summary>
        public ReligionEffect ActualEffect { get; set; }

        /// <summary>
        /// Gets the <see cref="Effects.IProvincionalEffect"/> taken into account when this <see cref="Religion"/> is the state religion.
        /// </summary>
        [NotMapped]
        public Effects.IProvincionalEffect Effect
        {
            get
            {
                if (this.effect == null)
                {
                    this.effect = this.ActualEffect != null ? this.ActualEffect.Simplify() : new Effects.ProvincionalEffect();
                }

                return this.effect;
            }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="ReligionInfluence"/> objects that benefit this <see cref="Religion"/>.
        /// </summary>
        public ICollection<ReligionInfluence> ReligionInfluences { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Religion"/> is currently the state religion.
        /// </summary>
        [NotMapped]
        public bool IsState
        {
            get
            {
                if (this.stateReligionTracker == null)
                {
                    throw new ReligionsException("State religion not tracked.");
                }

                return this.isState;
            }
        }

        /// <summary>
        /// Sets the <see cref="IStateReligionTracker"/> used by this <see cref="Religion"/> to update its state.
        /// </summary>
        [NotMapped]
        public IStateReligionTracker StateReligionTracker
        {
            set
            {
                this.stateReligionTracker = value;
                this.stateReligionTracker.StateReligionChanged += this.OnStateReligionChanged;
            }
        }

        /// <summary>
        /// Returns a value indicating whether the current combination of values is valid.
        /// </summary>
        /// <param name="message">
        /// Optional message containing details of vaildation's result.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> indicating result of validation.
        /// </returns>
        public bool Validate(out string message)
        {
            if (this.Name == null)
            {
                message = "Name is null.";
                return false;
            }

            if (this.Name.Equals(string.Empty))
            {
                message = "Name is empty.";
                return false;
            }

            if (!this.Effect.Validate(out string submessage))
            {
                message = $"Corresponding effect is in invalid ({submessage}).";
                return false;
            }

            message = "Values are valid.";
            return true;
        }

        /// <summary>
        /// Activities necessary to perform when the state religion changes.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void OnStateReligionChanged(object sender, StateReligionChangedArgs e)
        {
            this.isState = ReferenceEquals(this, e.NewStateReligion);
        }
    }
}