using System.Collections.Generic;
using GameWorld.Effects;
using NUnit.Framework;

namespace Tests.GameWorld.Effects
{
	[TestFixture]
	public class EffectsPackageTests
	{
		private ProvincionalEffectsPackage[] _templatePackages;

		[OneTimeSetUp]
		public void SetUp()
		{
			_templatePackages = new ProvincionalEffectsPackage[2];
			_templatePackages[0] = new ProvincionalEffectsPackage(10, 0, 0, 0, 0, 0, 0, 0, null);
			_templatePackages[1] = new ProvincionalEffectsPackage(0, 0, 0, 0, 0, 0, 0, 0, new List<WealthBonus>() {new WealthBonus(10, WealthCategory.All, BonusType.Percentage), new WealthBonus(1000, WealthCategory.Culture, BonusType.Simple)});
		}

		[TestCase("{'PublicOrder': 10}", 0)]
		[TestCase("{'WealthBonuses': [{'Value': 10, 'Type':'Percentage', 'Category': 'All'},{'Value': 1000, 'Type': 'Simple', 'Category': 'Culture'}]}", 1)]
		public void Correct(string json, int templateIndex)
		{
			ProvincionalEffectsPackage package = null;
			Assert.DoesNotThrow(() => package = ProvincionalEffectsPackage.Deserialize(json));
			Assert.AreEqual(_templatePackages[templateIndex], package);
		}
	}
}