namespace TWBuildingAssistant.Model.Map
{
    public interface IClimateParser
    {
        ClimateAndWeather.Climate Parse(string input);
    }
}
