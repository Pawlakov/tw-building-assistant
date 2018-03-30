using System;
namespace Simulator
{
	public enum BuildingType { TOWN, CENTERTOWN, CITY, CENTERCITY, COAST, RESOURCE, SPICE };
	internal static class Globals
	{
		static public readonly int minimalOrder;
		static public readonly int minimalSanitation;
		//
		static Globals()
		{
			Console.Clear();
			//
			Console.Write("Choose minimal public order: ");
			minimalOrder = Convert.ToInt32(Console.ReadLine());
			//
			Console.Write("Choose minimal sanitation: ");
			minimalSanitation = Convert.ToInt32(Console.ReadLine());
		}
		//
		public static int BuildingTypesCount
		{
			get { return 7; }
		}
	}
}