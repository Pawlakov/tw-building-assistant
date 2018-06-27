namespace GameWorld.Map
{
	public interface IClimateParser
	{
		ClimateAndWeather.Climate Parse(string input);
	}
}
