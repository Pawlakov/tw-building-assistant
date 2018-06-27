using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace TWBuildingAssistant.RandomSimulator
{
	public class Simulation
	{
		private GameWorld.Combinations.Template CombinationTemplate { get; }
		private Comparison<GameWorld.Combinations.Combination> CombinationComparison { get; }
		private SimulationSettings Settings { get; }
		private Round.NecessaryCondition Condition { get; }
		private GameWorld.Buildings.BuildingsPool BuildingPool { get; }
		private uint RoundSize { get; set; }
		//
		//
		private static int ParallelCount { get; } = System.Environment.ProcessorCount;
		//
		//
		public Simulation(GameWorld.Combinations.Template combinationTemplate, GameWorld.Buildings.BuildingsPool newBuildingPool, Comparison<GameWorld.Combinations.Combination> combinationComparison, SimulationSettings settings, Round.NecessaryCondition condition)
		{
			CombinationTemplate = combinationTemplate;
			CombinationComparison = combinationComparison;
			Settings = settings;
			BuildingPool = newBuildingPool;
			RoundSize = 0;
			Condition = condition;
		}
		public GameWorld.Combinations.Combination Run(GameWorld.World environment)
		{
			Round[] parallelRounds = new Round[ParallelCount];
			SortedSet<GameWorld.Combinations.Combination>[] roundsResults = new SortedSet<GameWorld.Combinations.Combination>[ParallelCount];
			SortedSet<GameWorld.Combinations.Combination> mergedResults = new SortedSet<GameWorld.Combinations.Combination>(new CombinationsComparer(CombinationComparison));
			int cursorPosition;
			//
			CurrentListSize = Settings.FirstListSize;
			Console.Clear();
			// Jedna iteracja to jedna "multirunda" czyli kilka rund równoległych.
			while (CurrentListSize > 1)
			{
				// Wykonanie równoległych rund.
				cursorPosition = Console.CursorTop;
				for(int whichParallelRound = 0; whichParallelRound < ParallelCount; ++whichParallelRound)
					parallelRounds[whichParallelRound] = new Round(CombinationTemplate, BuildingPool, CombinationComparison, new Round.RoundSettings { ConsoleLine = cursorPosition + whichParallelRound, ListSize = CurrentListSize, RoundSize = RoundSize }, Condition);
				Parallel.For(0, ParallelCount, 
					(int whichParallelRound) =>
					{
						roundsResults[whichParallelRound] = parallelRounds[whichParallelRound].Run(environment);
					});
				// Korekcja parametrów w razie konieczności.
				foreach (Round item in parallelRounds)
				{
					if (item.ValidCount > RoundSize)
						RoundSize = item.ValidCount;
					if (item.ValidCount < CurrentListSize)
						CurrentListSize = item.ValidCount;
				}
				// Łączenie wyników z równoległych rund.
				for (int whichResult = 0; whichResult < ParallelCount; ++whichResult)
					mergedResults.UnionWith(roundsResults[whichResult]);
				while ((uint)(mergedResults.Count) > CurrentListSize)
					mergedResults.Remove(mergedResults.Min);
				// Ocena przydatności budynków.
				foreach (GameWorld.Combinations.Combination combination in mergedResults)
					combination.RewardBuildings();
				BuildingPool.EvaluateBuildings();
				// Wyświetlenie obecnego stanu.
				ShowCurrentState(mergedResults.Max);
				// Zmniejszenie listy przed następną rundą.
				CurrentListSize = (uint)(CurrentListSize * Settings.RetainmentRate);
				while ((uint)(mergedResults.Count) > CurrentListSize)
					mergedResults.Remove(mergedResults.Min);
				++MultiroundsDoneCount;
			}
			return mergedResults.Max;
		}
		//
		//
		private uint MultiroundsDoneCount { get; set; }
		private uint CurrentListSize { get; set; }
		//
		//
		private void ShowCurrentState(GameWorld.Combinations.Combination bestValid)
		{
			Console.Clear();
			Console.WriteLine("Current state of building pool: ");
			BuildingPool.ShowCurrentState();
			Console.WriteLine("Best combination so far: ");
			bestValid.ShowContent(false);
			Console.WriteLine("Rounds done: {0} | Current list size: {1}", MultiroundsDoneCount, CurrentListSize);
		}
		//
		//
		public struct SimulationSettings
		{
			public uint FirstListSize { get; set; }
			public double RetainmentRate { get; set; }
		}
	}
}
