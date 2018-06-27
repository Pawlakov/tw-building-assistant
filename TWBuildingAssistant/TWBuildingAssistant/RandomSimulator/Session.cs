using System;
namespace TWBuildingAssistant.RandomSimulator
{
	public class Session
	{
		readonly uint firstListSize;
		readonly double retainmentRate;
		//
		public Session()
		{
			Console.Clear();
			Console.Write("Choose first list size: ");
			firstListSize = Convert.ToUInt32(Console.ReadLine());
			Console.Write("Choose retainemnt rate: ");
			retainmentRate = Convert.ToDouble(Console.ReadLine());
		}
		public int BetterInWealth(GameWorld.Combinations.Combination left, GameWorld.Combinations.Combination right)
		{
			if (left.Wealth > right.Wealth)
				return 1;
			if (left.Wealth < right.Wealth)
				return -1;
			return 0;
		}
		public int BetterInScience(GameWorld.Combinations.Combination left, GameWorld.Combinations.Combination right)
		{
			if (left.ResearchRate > right.ResearchRate)
				return 1;
			if (left.ResearchRate < right.ResearchRate)
				return -1;
			return BetterInWealth(left, right);
		}
		public int BetterInGrowth(GameWorld.Combinations.Combination left, GameWorld.Combinations.Combination right)
		{
			if (left.Growth > right.Growth)
				return 1;
			if (left.Growth < right.Growth)
				return -1;
			return BetterInWealth(left, right);
		}
		public void Run()
		{
			GameWorld.World environment;
			Simulation simulation;
			GameWorld.Combinations.Combination result;
			//
			bool useResource;
			Comparison<GameWorld.Combinations.Combination> combinationComparison;
			Round.NecessaryCondition condition = new Round.NecessaryCondition();
			//
			Console.Clear();
			Console.WriteLine("0. Maximal wealth simulation");
			Console.WriteLine("1. Maximal growth simulation");
			Console.WriteLine("2. Maximal research rate simulation");
			Console.Write("Pick: ");
			switch (Convert.ToInt32(Console.ReadLine()))
			{
				default:
					combinationComparison = BetterInWealth;
					break;
				case 1:
					combinationComparison = BetterInGrowth;
					break;
				case 2:
					combinationComparison = BetterInScience;
					break;
			}
			//
			Console.WriteLine("0. Don't harvest special resources");
			Console.WriteLine("1. Harvest special resources");
			Console.Write("Pick: ");
			if (Convert.ToInt32(Console.ReadLine()) == 1)
				useResource = true;
			else
				useResource = false;
			//
			environment = new GameWorld.World();
			environment.AskUserForSettings();
			//
			Console.Clear();
			Console.Write("Pick minimal sanitation: ");
			condition.MinimalSanitation = Convert.ToInt32(Console.ReadLine());
			Console.Write("Pick minimal food: ");
			condition.MinimalFood = Convert.ToInt32(Console.ReadLine());
			Console.Write("Pick minimal public order: ");
			condition.MinimalPublicOrder = Convert.ToInt32(Console.ReadLine());
			//
			simulation = new Simulation(environment.CreateCombinationTemplate(useResource), environment.CreateBuildingPool(), combinationComparison, new Simulation.SimulationSettings { FirstListSize = firstListSize, RetainmentRate = retainmentRate }, condition);
			result = simulation.Run(environment);
			Console.Clear();
			Console.WriteLine("Final solution:");
			result.ShowContent(false);
			Console.ReadKey();
		}
	}
}