namespace TWBuildingAssistant.Model
{
    public interface IParser<T>
    {
        T Parse(string input);

        T Find(int? id);
    }
}