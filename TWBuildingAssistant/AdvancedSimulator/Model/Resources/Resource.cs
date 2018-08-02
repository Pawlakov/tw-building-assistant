namespace TWBuildingAssistant.Model.Resources
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents one of in-game special resources.
    /// </summary>
    [Table("Resources")]
    public class Resource : IResource
    {
        /// <summary>
        /// Gets or sets the primary key of this resource.
        /// </summary>
        [Key]
        public int ResourceId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Resource"/>'s corresponding buildings are obligatory to build.
        /// </summary>
        [Column]
        [Required]
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Gets or sets this <see cref="Resource"/>'s <see cref="SlotType"/> of corresponding buildings.
        /// </summary>
        [Column]
        [Required]
        public SlotType BuildingType { get; set; }

        /// <summary>
        /// Gets or sets the name of this <see cref="Resource"/>
        /// </summary>
        [Column]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Returns a value indicating whether the current combination of values is valid.
        /// </summary>
        /// <param name="message">
        /// Optional message containing details of vaildation's result.
        /// </param>
        /// <returns>
        /// The <see cref="T:System.Boolean" /> indicating result of validation.
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

            if (this.BuildingType == SlotType.Regular && this.IsMandatory)
            {
                message = "Mandatory replacement of general building.";
                return false;
            }

            message = "Values are valid.";
            return true;
        }

        /// <summary>
        /// Returns this <see cref="Resource"/>'s <see cref="string"/> representaion (which is its name).
        /// </summary>
        /// <returns>
        /// The <see cref="string"/> representation.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}