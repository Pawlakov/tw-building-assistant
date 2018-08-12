namespace TWBuildingAssistant.Model.Religions
{
    using TWBuildingAssistant.Model.Effects;

    /// <summary>
    /// Represents one of in-game religions.
    /// </summary>
    public interface IReligion
    {
        /// <summary>
        /// Gets the name of this <see cref="IReligion"/>
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="IProvincionalEffect"/> taken into account when this <see cref="IReligion"/> is the state religion.
        /// </summary>
        IProvincionalEffect Effect { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IReligion"/> is currently the state religion.
        /// </summary>
        bool IsState { get; }

        /// <summary>
        /// Sets the <see cref="IStateReligionTracker"/> used by this <see cref="IReligion"/> to update its state.
        /// </summary>
        IStateReligionTracker StateReligionTracker { set; }

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
