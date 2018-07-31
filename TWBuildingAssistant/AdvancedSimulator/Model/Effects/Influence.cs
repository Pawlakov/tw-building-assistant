namespace TWBuildingAssistant.Model.Effects
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Influence : IInfluence
    {
        public virtual Religions.IReligion Religion { get; set; }

        [Column]
        [Required]
        public int Value { get; set; }

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
    }
}