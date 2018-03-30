namespace Simulator
{
	public class Session
	{
		public Session()
		{
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
		}
	}
}
