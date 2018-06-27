using System;
using System.Collections.Generic;
namespace TWBuildingAssistant.RandomSimulator
{
	class CombinationsComparer : IComparer<GameWorld.Combinations.Combination>
	{
		private readonly Comparison<GameWorld.Combinations.Combination> _comparison;
		public CombinationsComparer(Comparison<GameWorld.Combinations.Combination> comparison)
		{
			_comparison = comparison;
		}
		public int Compare(GameWorld.Combinations.Combination x, GameWorld.Combinations.Combination y)
		{
			return _comparison(x, y);
		}
	}
}
