using System;
namespace Simulator
{
	internal abstract class Round
	{
		// Stan wewnętrzny:
		//
		protected RoundEndCondition _loopCondition;
		protected Predicate<ProvinceCombination> _validationCondidtion;
		protected ProvinceTemplate Template { get; }
		protected BuildingPool Pool { get; }
		protected int Size { get; }
		protected CombinationsCollection _results;
		internal Round(Predicate<ProvinceCombination> validationCondition, ProvinceTemplate template, BuildingPool pool, int index, int size)
		{
			Size = size;
			Index = index;
			Template = template;
			Pool = pool;
		}
		public void Run()
		{
			uint allCount = 0;
			uint validCount = 0;
			string footer;
			CombinationsCollection result = new SortedSet<ProvinceCombination>(new CombinationsComparator(BetterInWealth));
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
				if (roundSize > 0)
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
		public int Index { get; }
	}
}
