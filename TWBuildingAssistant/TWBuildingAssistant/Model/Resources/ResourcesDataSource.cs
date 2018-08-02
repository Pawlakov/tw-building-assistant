namespace TWBuildingAssistant.Model.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides acces to all <see cref="IResource"/> objects found in the datbase.
    /// </summary>
    public class ResourcesDataSource : IResourcesSource
    {
        /// <summary>
        /// The database context containing information on resources.
        /// </summary>
        private readonly Data.DataModel context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourcesDataSource"/> class.
        /// </summary>
        /// <param name="context">
        /// The database context containing information on resources.
        /// </param>
        public ResourcesDataSource(Data.DataModel context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets all resources from the database.
        /// </summary>
        /// <returns>
        /// All <see cref="IResource"/> objects from the database.
        /// </returns>
        public IEnumerable<IResource> GetResources()
        {
            return (from Resource item in this.context.Resources select item).ToArray();
        }
    }
}