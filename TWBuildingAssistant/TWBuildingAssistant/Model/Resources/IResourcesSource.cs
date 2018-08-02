namespace TWBuildingAssistant.Model.Resources
{
    using System.Collections.Generic;

    /// <summary>
    /// Exposes acces to all <see cref="IResource"/> object from certian source.
    /// </summary>
    public interface IResourcesSource
    {
        /// <summary>
        /// Gets all resources from this source.
        /// </summary>
        /// <returns>
        /// All <see cref="IResource"/> objects from this source.
        /// </returns>
        IEnumerable<IResource> GetResources();
    }
}