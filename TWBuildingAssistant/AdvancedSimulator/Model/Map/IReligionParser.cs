namespace TWBuildingAssistant.Model.Map
{
    public interface IReligionParser
    {
        Religions.IReligion Parse(string input);
    }
}
