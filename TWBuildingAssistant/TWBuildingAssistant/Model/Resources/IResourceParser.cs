namespace TWBuildingAssistant.Model.Resources
{
    public interface IResourceParser
    {
        Resources.IResource Parse(string input);

        Resources.IResource Find(int id);
    }
}
