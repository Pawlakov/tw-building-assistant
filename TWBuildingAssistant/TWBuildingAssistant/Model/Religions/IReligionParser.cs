namespace TWBuildingAssistant.Model.Religions
{
    public interface IReligionParser
    {
        IReligion Parse(string input);

        IReligion Find(int? id);
    }
}
