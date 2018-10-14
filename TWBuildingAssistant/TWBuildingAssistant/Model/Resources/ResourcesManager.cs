namespace TWBuildingAssistant.Model.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Unity;

    public class ResourcesManager : IParser<IResource>
    {
        private readonly IEnumerable<IResource> resources;

        public ResourcesManager(IUnityContainer resolver)
        {
            this.resources = resolver.Resolve<ISource>().Resources.ToArray();
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

        public IResource Find(int? id)
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