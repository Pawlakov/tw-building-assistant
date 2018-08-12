namespace TWBuildingAssistant.Model.Religions
{
    using System.Collections.Generic;

    /// <summary>
    /// Exposes acces to all <see cref="IReligion"/> object from certian source.
    /// </summary>
    public interface IReligionsSource
    {
        /// <summary>
        /// Gets all religions from this source.
        /// </summary>
        /// <returns>
        /// All <see cref="IReligion"/> objects from this source.
        /// </returns>
        IEnumerable<IReligion> GetReligions();
    }
}