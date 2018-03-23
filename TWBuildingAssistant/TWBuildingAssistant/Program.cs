using System;
namespace TWBuildingAssistant
{
	class Program
	{
		public static void Main()
		{
			ClimateAndWeather.Climate myClimate;
			myClimate = ClimateAndWeather.ClimateManager.Singleton.Parse("Medium");
			try
			{
				Console.WriteLine("Porządek publiczny dla MEDIUM: {0}", myClimate.PublicOrder);
			}
			catch(Exception exception)
			{
				Console.WriteLine("Nie odczytałem orządku publicznego dla MEDIUM.");
				Console.WriteLine("Wyjątek: {0}", exception.Message);
			}
			ClimateAndWeather.WeatherManager.Singleton.ChangeWorstCaseWeather();
			try
			{
				Console.WriteLine("Porządek publiczny dla MEDIUM: {0}", myClimate.PublicOrder);
			}
			catch (Exception exception)
			{
				Console.WriteLine("Nie odczytałem orządku publicznego dla MEDIUM.");
				Console.WriteLine("Wyjątek: {0}", exception.ToString());
			}
			Console.ReadKey();
		}
	}
}