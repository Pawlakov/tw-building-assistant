using System.Collections.Generic;
using GameWorld.Effects;
using NUnit.Framework;

namespace Tests.GameWorld.Effects
{
	[TestFixture]
	public class WealthTests
	{
		[Test]
		public void AllTypesOnCategory
		(
		[Values(0, 6)] int fertilityLevel,
		[Values(0, 1650)] int firstValue,
		[Values(0, 20)] int secondValue,
		[Values(0, 165)] int thirdValue,
		[Values(WealthCategory.Agriculture, WealthCategory.Husbandry)]
		WealthCategory category
		)
		{
			var wealth = new Wealth();
			var expectedValue = (firstValue + thirdValue * fertilityLevel) * 0.01 * (100 + secondValue);
			//
			wealth.AddBonus(new WealthBonus(firstValue, category, BonusType.Simple));
			wealth.AddBonus(new WealthBonus(secondValue, category, BonusType.Percentage));
			wealth.AddBonus(new WealthBonus(thirdValue, category, BonusType.FertilityDependent));
			Assert.AreEqual(expectedValue, wealth.TotalWealth(fertilityLevel), 0.01);
		}

		[Test]
		public void AllMultiplier()
		{
			var wealth = new Wealth();
			var expectedValue = 0;
			wealth.AddBonus(new WealthBonus(10, WealthCategory.All, BonusType.Percentage));
			foreach (var category in Wealth.Categories)
				if (category != WealthCategory.All && category != WealthCategory.Maintenance)
				{
					wealth.AddBonus(new WealthBonus(100, category, BonusType.Simple));
					expectedValue += 110;
				}

			Assert.AreEqual(expectedValue, wealth.TotalWealth(0), 0.01);
		}

		[Test]
		public void ArrayOfBonuses()
		{
			var wealth = new Wealth();
			var bonuses = new List<WealthBonus>();
			var expectedValue = 0;
			bonuses.Add(new WealthBonus(10, WealthCategory.All, BonusType.Percentage));
			foreach (var category in Wealth.Categories)
				if (category != WealthCategory.All && category != WealthCategory.Maintenance)
				{
					bonuses.Add(new WealthBonus(100, category, BonusType.Simple));
					expectedValue += 110;
				}

			wealth.AddBonuses(bonuses);
			Assert.AreEqual(expectedValue, wealth.TotalWealth(0), 0.01);
		}
	}
}