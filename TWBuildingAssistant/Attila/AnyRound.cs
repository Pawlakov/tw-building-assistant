using System;
namespace Simulator
{
	internal class AnyRound : Round
	{
		internal AnyRound(Predicate<ProvinceCombination> validationCondition, ProvinceTemplate template, BuildingPool pool, int index, int size) : base(validationCondition, template, pool, index, size)
		{
			_loopCondition = (uint allCount, uint validCount) => { return (validCount % Size) != 0; };
		}
	}
}
