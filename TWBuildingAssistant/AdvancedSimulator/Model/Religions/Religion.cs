namespace TWBuildingAssistant.Model.Religions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using TWBuildingAssistant.Model.Map;

    [Table("Religions")]
    public class Religion : IReligion
    {
        private Map.IStateReligionTracker stateReligionTracker;

        private bool isState;

        private Effects.IProvincionalEffect effect;

        [Key]
        public int ReligionId { get; set; }

        [Column]
        [Required]
        public string Name { get; set; }

        public ReligionEffect ActualEffect { get; set; }

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

        public ICollection<ReligionInfluence> ReligionInfluences { get; set; }

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

            message = "Values are valid.";
            return true;
        }

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

        [NotMapped]
        public IStateReligionTracker StateReligionTracker
        {
            set
            {
                this.stateReligionTracker = value;
                this.stateReligionTracker.StateReligionChanged += (sender, e) =>
                    {
                        this.isState = ReferenceEquals(this, sender.StateReligion);
                    };
            }
        }
    }
}