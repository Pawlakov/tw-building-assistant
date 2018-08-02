namespace TWBuildingAssistant.Model.Resources
{
    /// <summary>
    /// Represents one of in-game special resources.
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// Gets the name of this <see cref="IResource"/>
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets this <see cref="IResource"/>'s <see cref="SlotType"/> of corresponding buildings.
        /// </summary>
        SlotType BuildingType { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IResource"/>'s corresponding buildings are obligatory to build.
        /// </summary>
        bool IsMandatory { get; }

        /// <summary>
        /// Returns a value indicating whether the current combination of values is valid.
        /// </summary>
        /// <param name="message">
        /// Optional message containing details of vaildation's result.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> indicating result of validation.
        /// </returns>
        bool Validate(out string message);
    }
}