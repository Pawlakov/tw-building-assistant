namespace TWBuildingAssistant.Model.Resources
{
    using System.Collections.Generic;

    public interface IResourcesSource
    {
        IEnumerable<IResource> Resources { get; }
    }
}