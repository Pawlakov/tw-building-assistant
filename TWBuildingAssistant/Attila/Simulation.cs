using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
namespace Simulator
{
	internal class Simulation
	{
		uint roundSize;
		uint currentListSize;
		uint roundsDoneCount;
		int cursorPosition;
		object threadLock;
		readonly uint firstListSize;
		readonly double retainmentRate;
		readonly int threadsCount;
		//
		public Simulation()
		{
			Console.Clear();
			threadLock = new object();
			Console.Write("Choose first list size: ");
			firstListSize = Convert.ToUInt32(Console.ReadLine());
			Console.Write("Choose retainemnt rate: ");
			retainmentRate = Convert.ToDouble(Console.ReadLine());
			Console.Write("Choose thread count: ");
			threadsCount = Convert.ToInt32(Console.ReadLine());
		}
		public int BetterInWealth(ProvinceCombination left, ProvinceCombination right)
		{
			if (left.Wealth > right.Wealth)
				return 1;
			if (left.Wealth < right.Wealth)
				return -1;
			return 0;
		}
		public int BetterInScience(ProvinceCombination left, ProvinceCombination right)
		{
			if (left.Science > right.Science)
				return 1;
			if (left.Science < right.Science)
				return -1;
			return BetterInWealth(left, right);
		}
		public int BetterInGrowth(ProvinceCombination left, ProvinceCombination right)
		{
			if (left.Growth > right.Growth)
				return 1;
			if (left.Growth < right.Growth)
				return -1;
			return BetterInWealth(left, right);
		}
		public ProvinceCombination GenerateOneCombination(ProvinceCombination template, Comparison<ProvinceCombination> comparison)
		{
			// Every generation requires its own library due to building rewarding system.
			BuildingLibrary library = new BuildingLibrary(Globals.faction.Buildings);
			// List for every parallel thread.
			SortedSet<ProvinceCombination>[] threadsResults = new SortedSet<ProvinceCombination>[threadsCount];
			// Merged list of best valid combinations.
			SortedSet<ProvinceCombination> mergedResults = new SortedSet<ProvinceCombination>(new CombinationsComparator(comparison));
			// Current best (later also best overall).
			ProvinceCombination bestValid = null;
			//Current length of valid list.
			currentListSize = firstListSize;
			roundsDoneCount = 0;
			cursorPosition = 0;
			roundSize = 0;
			//
			Console.Clear();
			while (currentListSize > 1)
			{
				// Parallel rounds.
				cursorPosition = Console.CursorTop;
				Parallel.For(0, threadsCount, (int whichResult) => threadsResults[whichResult] = AnyRound(template, library, cursorPosition + whichResult));
				// Merging parallel results.
				for (int whichResult = 0; whichResult < threadsCount; ++whichResult)
					mergedResults.UnionWith(threadsResults[whichResult]);
				// Removing overflow in merged list.
				while ((uint)(mergedResults.Count) > currentListSize)
					mergedResults.Remove(mergedResults.Min);
				// Remembering the best.
				bestValid = mergedResults.Max;
				// Rewarding succesful buildings.
				foreach (ProvinceCombination combination in mergedResults)
					combination.RewardBuildings();
				library.EvaluateBuildings();
				// Showing current round's state.
				Console.Clear();
				Console.WriteLine("COAST levels left: {0}", library.GetCountByType(BuildingType.COAST));
				Console.WriteLine("CITY levels left: {0}", library.GetCountByType(BuildingType.CITY));
				Console.WriteLine("TOWN levels left: {0}", library.GetCountByType(BuildingType.TOWN));
				Console.WriteLine("Best so far: ");
				bestValid.ShowContent(false);
				Console.WriteLine("Rounds done: {0} | Current list size: {1}", roundsDoneCount, currentListSize);
				// 
				currentListSize = (uint)(currentListSize * retainmentRate);
				while ((uint)(mergedResults.Count) > currentListSize)
					mergedResults.Remove(mergedResults.Min);
				++roundsDoneCount;
			}
			return bestValid;
		}
		public bool MinimalCondition(ProvinceCombination subject)
		{
			return (subject.Order >= Globals.minimalOrder
					&& subject.Food >= 0
					&& subject.getSanitation(0) >= Globals.minimalSanitation
					&& subject.getSanitation(1) >= Globals.minimalSanitation
					&& subject.getSanitation(2) >= Globals.minimalSanitation);
		}
	}
	class CombinationsComparator : IComparer<ProvinceCombination>
	{
		readonly Comparison<ProvinceCombination> comparison;
		public CombinationsComparator(Comparison<ProvinceCombination> comparison)
		{
			this.comparison = comparison;
		}
		public int Compare(ProvinceCombination x, ProvinceCombination y)
		{
			return comparison(x, y);
		}
	}
	delegate bool RoundEndCondition(uint allCount, uint validCount);
}