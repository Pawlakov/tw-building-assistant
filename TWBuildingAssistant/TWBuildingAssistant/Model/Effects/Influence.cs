namespace TWBuildingAssistant.Model.Effects
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Respresents an in-game religious influence.
    /// </summary>
    public class Influence : IInfluence
    {
        /// <summary>
        /// Gets or sets the religion of this influence.
        /// </summary>
        public virtual Religions.IReligion Religion { get; set; }

        /// <summary>
        /// Gets or sets the value of this religious influence.
        /// </summary>
        [Column]
        [Required]
        public int Value { get; set; }

        /// <summary>
        /// Returns a value indicating whether the current combination of values is valid.
        /// </summary>
        /// <param name="message">
        /// Optional message containing details of vaildation's result.
        /// </param>
        /// <returns>
        /// The <see cref="bool" /> indicating result of validation.
        /// </returns>
        public bool Validate(out string message)
        {
            if (this.Value < 1)
            {
                message = "Value is nonpositive.";
                return false;
            }

            message = "Values are correct.";
            return true;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="Influence"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/> representing this <see cref="Influence"/>.
        /// </returns>
        public override string ToString()
        {
            return $"+{this.Value} {this.Religion?.ToString() ?? "state religion"} religious influence";
        }
    }
}