namespace TWBuildingAssistant.Model.Effects
{
    /// <summary>
    /// Represents a set of effects acting on a region.
    /// </summary>
    public interface IRegionalEffect : IProvincionalEffect
    {
        /// <summary>
        /// Gets the change of sanitation in only one region.
        /// </summary>
        int? RegionalSanitation { get; }

        /// <summary>
        /// Adds this effect to another one.
        /// </summary>
        /// <param name="other">
        /// The other component of addition.
        /// </param>
        /// <returns>
        /// The <see cref="IRegionalEffect"/> resulting from the addition.
        /// </returns>
        IRegionalEffect Aggregate(IRegionalEffect other);
    }
}