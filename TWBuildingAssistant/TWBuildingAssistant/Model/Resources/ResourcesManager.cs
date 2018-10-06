namespace TWBuildingAssistant.Model.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ResourcesManager : IResourceParser
    {
        private readonly IEnumerable<IResource> resources;

        public ResourcesManager(IResourcesSource source)
        {
            this.resources = source.Resources.ToArray();
            var message = string.Empty;
            if (this.resources.Any(resource => !resource.Validate(out message)))
            {
                throw new ResourcesException($"One of resources is not valid ({message}).");
            }
        }

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

        public IResource Find(int id)
        {
            var result = this.resources.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                throw new ResourcesException("No matching resource found.");
            }

            return result;
        }
    }
}