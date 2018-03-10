namespace TWBuildingAssistant
{
	class Program
	{
		public static void Main()
		{
			Utilities.XorShift randomEngine = new Utilities.XorShift();
			int[] table = new int[10];
			for (int i = 0; i < 10; ++i)
				table[i] = 0;
			//
			int outcome;
			for (int i = 0; i < 0x00ffffff; ++i)
			{
				outcome = randomEngine.Next(0, 10);
				table[outcome] += 1;
			}
			//
			for (int i = 0; i < 10; ++i)
				System.Console.WriteLine(table[i]);
			System.Console.ReadKey();
		}
	}
}