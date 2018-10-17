namespace TWBuildingAssistant.Model.Resources
{
    using System;
    using System.Linq;

    using Unity;

    public class ResourceManager : Parser<IResource>
    {
        public ResourceManager(IUnityContainer resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            try
            {
                this.Content = resolver.Resolve<ISource>().Resources.ToArray();
            }
            catch (Exception e)
            {
                throw new ResourceException("Failed to get the resources from the source.", e);
            }

            var message = string.Empty;
            if (this.Content.Any(resource => !resource.Validate(out message)))
            {
                throw new ResourceException($"One of resources is not valid ({message}).");
            }
        }
    }
}