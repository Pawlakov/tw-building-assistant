using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
namespace TWAssistant
{
	namespace Attila
	{
		public class Simulator
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
			public Simulator()
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
			public void Act()
			{
				ProvinceData province;
				ProvinceCombination result;
				//
				Console.Clear();
				Globals.map.ShowList();
				Console.Write("Pick a province: ");
				province = Globals.map[Convert.ToUInt32(Console.ReadLine())];
				//
				Console.Clear();
				Console.WriteLine("0. Base simulation");
				Console.WriteLine("1. Resource simulation");
				Console.WriteLine("2. Growth simulation");
				Console.WriteLine("3. Science simulation");
				Console.Write("Pick a type of simulation: ");
				switch (Convert.ToUInt32(Console.ReadLine()))
				{
					default:
						result = GenerateOneCombination(new ProvinceCombination(province, false), BetterInWealth);
						break;
					case 1:
						result = GenerateOneCombination(new ProvinceCombination(province, true), BetterInWealth);
						break;
					case 2:
						result = GenerateOneCombination(new ProvinceCombination(province, false), BetterInGrowth);
						break;
					case 3:
						result = GenerateOneCombination(new ProvinceCombination(province, false), BetterInScience);
						break;
				}
				Console.Clear();
				Console.WriteLine("Final solution:");
				result.ShowContent(false);
				Console.ReadKey();
			}
			public void GenerateFullProvince(ProvinceData province)
			{
				ProvinceCombination baseTemplate = new ProvinceCombination(province, false);
				ProvinceCombination resourceTemplate = new ProvinceCombination(province, true);
				ProvinceCombination baseCombination = GenerateOneCombination(baseTemplate, BetterInWealth);
				ProvinceCombination resourceCombination = GenerateOneCombination(resourceTemplate, BetterInWealth);
				ProvinceCombination growthCombination = GenerateOneCombination(baseTemplate, BetterInGrowth);
				ProvinceCombination scienceCombination = GenerateOneCombination(baseTemplate, BetterInScience);
				//ProvinceCombination growthResourceCombination = GenerateOneCombination(resourceTemplate, BetterInGrowth);
				//ProvinceCombination scienceResourceCombination = GenerateOneCombination(resourceTemplate, BetterInScience);
				StreamWriter stream = new StreamWriter(province.Name + ".txt");
				stream.WriteLine("Base");
				stream.WriteLine(baseCombination);
				stream.WriteLine("Resource");
				stream.WriteLine(resourceCombination);
				stream.WriteLine("Growth");
				stream.WriteLine(growthCombination);
				stream.WriteLine("Science");
				stream.WriteLine(scienceCombination);
				//stream.WriteLine("ResourceGrowth");
				//stream.WriteLine(growthResourceCombination);
				//stream.WriteLine("ResourceScience");
				//stream.WriteLine(scienceResourceCombination);
				stream.Dispose();
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
					Parallel.For(0, threadsCount, (int whichResult) => threadsResults[whichResult] = Round(template, library, cursorPosition + whichResult));
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
			SortedSet<ProvinceCombination> Round(ProvinceCombination template, BuildingLibrary library, int cursorLine)
			{
				try
				{
					// General case.
					if (roundSize > 0)
					{
						return ActualRound((uint allCount, uint validCount) => (validCount % roundSize != 0), template, library, cursorLine);
					}
					// First round case (round size evaluation).
					return ActualRound((uint allCount, uint validCount) => (allCount < 0x00800000 && validCount < 0x00020000), template, library, cursorLine);
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception);
					Console.ReadKey();
				}
				return null;
			}
			SortedSet<ProvinceCombination> ActualRound(RoundEndCondition endCondition, ProvinceCombination template, BuildingLibrary library, int cursorLine)
			{
				uint allCount = 0;
				uint validCount = 0;
				string footer;
				SortedSet<ProvinceCombination> result = new SortedSet<ProvinceCombination>(new CombinationsComparator(BetterInWealth));
				XorShift random = new XorShift((uint)(Guid.NewGuid().GetHashCode()));
				//
				// Outer loop (do until whole round is done which is certain ammount of valid combinations)
				do
				{
					// Inner loop (do until one valid combination is found)
					while (true)
					{
						ProvinceCombination subject = new ProvinceCombination(template);
						++allCount;
						subject.Fill(random, library);
						if (MinimalCondition(subject))
						{
							++validCount;
							result.Add(subject);
							if ((uint)(result.Count) > currentListSize)
								result.Remove(result.Min);
							break;
						}
					}
					// Showing current state.
					if(roundSize > 0)
						footer = string.Format("All: {0:0,0} | Valid: {1:0,0}/{2:0,0} | All / Valid: {3:0,0} | List: {4:0,0}/{5:0,0}----", allCount, validCount, roundSize, allCount / validCount, result.Count, currentListSize);
					else
						footer = string.Format("All: {0:0,0}/8 388 608 | Valid: {1:0,0}/131 072 | All / Valid: {2:0,0} | List: {3:0,0}/{4:0,0}----", allCount, validCount, allCount / validCount, result.Count, currentListSize);
					lock (threadLock)
					{
						Console.SetCursorPosition(0, cursorLine);
						Console.Write(footer);
					}
				} while (endCondition(allCount, validCount));
				// Setting required parameters after first round.
				if (roundSize < 1)
				{
					if (roundSize < validCount)
						roundSize = validCount;
					if (currentListSize > roundSize)
						currentListSize = roundSize;
				}
				return result;
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
}