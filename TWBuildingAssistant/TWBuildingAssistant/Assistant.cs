using System;

namespace TWAssistant
{
	class Assistant
	{
		public static void Main()
		{
			Attila.Simulator simulator = new Attila.Simulator();
			try
			{
				simulator.Act();
			}
			catch(Exception exception)
			{
				Console.WriteLine(exception);
				Console.ReadKey();
			}
		}
	}
}