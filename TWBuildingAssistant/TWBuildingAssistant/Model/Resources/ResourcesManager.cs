namespace TWBuildingAssistant.Model.Resources
{
    using System.Collections.Generic;
    using System.Linq;

    using Unity;

    public class ResourcesManager : Parser<IResource>
    {
        public ResourcesManager(IUnityContainer resolver)
        {
            this.Content = resolver.Resolve<ISource>().Resources.ToArray();
            var message = string.Empty;
            if (this.Content.Any(resource => !resource.Validate(out message)))
            {
                throw new ResourcesException($"One of resources is not valid ({message}).");
            }
        }
    }
}