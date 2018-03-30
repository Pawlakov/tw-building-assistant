namespace Simulator
{
	internal class FirstRound : Round
	{
		private const int _maximalAllCombinationsCount = 0x00800000;
		private const int _maximalValidCombinationsCount = 0x00020000;
		internal FirstRound(Predicate<ProvinceCombination> validationCondition, ProvinceTemplate template, BuildingPool pool, int index, ref int size) : base(validationCondition, template, pool, index, size)
		{
			_loopCondition = (uint allCount, uint validCount) => { return allCount < _maximalAllCombinationsCount && validCount < _maximalValidCombinationsCount; };
		}
	}
}
