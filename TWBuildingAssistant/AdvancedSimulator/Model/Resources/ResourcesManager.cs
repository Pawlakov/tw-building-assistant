namespace TWBuildingAssistant.Model.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Manages all available in game special resources.
    /// </summary>
    public class ResourcesManager : Map.IResourceParser
    {
        /// <summary>
        /// All available in-game special resources.
        /// </summary>
        private readonly IEnumerable<IResource> resources;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourcesManager"/> class using a given source of resources.
        /// </summary>
        /// <param name="source">
        /// The source of <see cref="IResource"/> objects.
        /// </param>
        /// <exception cref="ResourcesException">
        /// Thrown if at least one of <see cref="IResource"/> objects provided by the source is not valid.
        /// </exception>
        public ResourcesManager(IResourcesSource source)
        {
            this.resources = source.GetResources();
            var message = string.Empty;
            if (this.resources.Any(resource => !resource.Validate(out message)))
            {
                throw new ResourcesException($"One of resources is not valid ({message}).");
            }
        }

        /// <summary>
        /// Gets all available in game special resources.
        /// </summary>
        public IEnumerable<IResource> Resources => this.resources.ToArray();

        /// <summary>
        /// Converts the <see cref="string"/> representing an <see cref="IResource"/> (its name) to and equivalent object.
        /// </summary>
        /// <param name="input">
        /// A <see cref="string"/> containing the name to convert.
        /// </param>
        /// <returns>
        /// The <see cref="IResource"/> with a matching name.
        /// </returns>
        public IResource Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var result = this.resources.FirstOrDefault(element => input.Equals(element.Name, StringComparison.OrdinalIgnoreCase));
            if (result == null)
            {
                throw new ResourcesException("No matching resource found.");
            }

            return result;
        }
    }
}