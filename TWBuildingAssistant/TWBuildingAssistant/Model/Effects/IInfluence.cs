namespace TWBuildingAssistant.Model.Effects
{
    /// <summary>
    /// Respresents an in-game religious influence.
    /// </summary>
    public interface IInfluence
    {
        /// <summary>
        /// Gets the religion of this influence. Null value means always state religion.
        /// </summary>
        Religions.IReligion Religion { get; }

        /// <summary>
        /// Gets the value of this religious influence.
        /// </summary>
        int Value { get; }

        /// <summary>
        /// Returns a value indicating whether the current combination of values is valid.
        /// </summary>
        /// <param name="message">
        /// Optional message containing details of vaildation's result.
        /// </param>
        /// <returns>
        /// The <see cref="bool" /> indicating result of validation.
        /// </returns>
        bool Validate(out string message);
    }
}