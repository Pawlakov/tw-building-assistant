using System;
using System.Timers;
using System.Collections.Generic;
namespace TWBuildingAssistant.RandomSimulator
{
	public class Round
	{
		private const double _statusUpdateInterval = 500.0;
		private const uint _maximalCreationSize = 0x00400000;
		private const uint _maximalRoundSize = 0x00010000;
		//
		//
		private GameWorld.Utilities.XorShift RandomEngine { get; }
		private GameWorld.Combinations.Template CombinationTemplate { get; }
		private GameWorld.Buildings.BuildingsPool BuildingPool { get; }
		private Comparison<GameWorld.Combinations.Combination> CombinationComparison { get; }
		private RoundSettings Settings { get; }
		private NecessaryCondition Condition { get; }
		private RoundEndCondition EndCondition { get; }
		//
		// Element współdzielony.
		private static object ParallelLock { get; }
		static Round()
		{
			ParallelLock = new object();
		}
		//
		//
		public Round(GameWorld.Combinations.Template combinationTemplate, GameWorld.Buildings.BuildingsPool buildingPool, Comparison<GameWorld.Combinations.Combination> combinationComparison, RoundSettings settings, NecessaryCondition condition)
		{
			RandomEngine = new GameWorld.Utilities.XorShift();
			CombinationTemplate = combinationTemplate;
			BuildingPool = buildingPool;
			CombinationComparison = combinationComparison;
			Settings = settings;
			//
			if (Settings.RoundSize > 0)
				EndCondition = (uint allCount, uint validCount) => (validCount % Settings.RoundSize != 0);
			else
				EndCondition = (uint allCount, uint validCount) => (allCount < _maximalCreationSize && validCount < _maximalRoundSize);
		}
		public SortedSet<GameWorld.Combinations.Combination> Run(GameWorld.World environment)
		{
			ValidCombinationsGenerated = new SortedSet<GameWorld.Combinations.Combination>(new CombinationsComparer(CombinationComparison));
			Timer statusUpdateTimer = new Timer(_statusUpdateInterval);
			statusUpdateTimer.Elapsed += StatusUpdateTimerAction;
			statusUpdateTimer.Start();
			do
			{
				GameWorld.Combinations.Combination subject = GenerateOneValid(environment);
			} while (EndCondition(AllCount, ValidCount));
			statusUpdateTimer.Stop();
			return ValidCombinationsGenerated;
		}
		//
		// Aktualny stan rundy:
		public uint ValidCount { get; private set; }
		public uint AllCount { get; private set; }
		private SortedSet<GameWorld.Combinations.Combination> ValidCombinationsGenerated { get; set; }
		//
		// Metody pomocnicze:
		private GameWorld.Combinations.Combination GenerateOneValid(GameWorld.World environment)
		{
			while (true)
			{
				GameWorld.Combinations.Combination result = CombinationTemplate.FillRandomly(RandomEngine, BuildingPool);
				++AllCount;
				result.Calculate(environment);
				if (Condition.CheckCondition(result))
				{
					++ValidCount;
					ValidCombinationsGenerated.Add(result);
					if ((uint)(ValidCombinationsGenerated.Count) > Settings.ListSize)
						ValidCombinationsGenerated.Remove(ValidCombinationsGenerated.Min);
					return result;
				}
			}
		}
		private void StatusUpdateTimerAction(object sender, ElapsedEventArgs e)
		{
			string footer;
			if (Settings.RoundSize > 0)
				footer = string.Format("All: {0:0,0} | Valid: {1:0,0}/{2:0,0} | All / Valid: {3:0,0} | List: {4:0,0}/{5:0,0}----", AllCount, ValidCount, Settings.RoundSize, (ValidCount == 0 ? 0 : AllCount / ValidCount), ValidCombinationsGenerated.Count, Settings.ListSize);
			else
				footer = string.Format("All: {0:0,0}/{1:0,0} | Valid: {2:0,0}/{3:0,0} | All / Valid: {4:0,0} | List: {5:0,0}/{6:0,0}----", AllCount, _maximalCreationSize, ValidCount, _maximalRoundSize, (ValidCount == 0 ? 0 : AllCount / ValidCount), ValidCombinationsGenerated.Count, Settings.ListSize);
			lock (ParallelLock)
			{
				Console.SetCursorPosition(0, Settings.ConsoleLine);
				Console.Write(footer);
			}
		}
		//
		//
		public struct RoundSettings
		{
			public int ConsoleLine { get; set; }
			public uint ListSize { get; set; }
			public uint RoundSize { get; set; }
		}
		public struct NecessaryCondition
		{
			public int MinimalFood { get; set; }
			public int MinimalPublicOrder { get; set; }
			public int MinimalSanitation { get; set; }
			public bool CheckCondition(GameWorld.Combinations.Combination combinaiton)
			{
				return 
					MinimalFood <= combinaiton.Food && 
					MinimalPublicOrder <= combinaiton.PublicOrder && 
					MinimalSanitation <= combinaiton.GetSanitation(0) &&
					MinimalSanitation <= combinaiton.GetSanitation(1) &&
					MinimalSanitation <= combinaiton.GetSanitation(2);
			}
		}
		public delegate bool RoundEndCondition(uint allCount, uint validCount);
	}
}