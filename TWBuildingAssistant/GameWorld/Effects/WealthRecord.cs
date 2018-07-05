namespace GameWorld.Effects
{
	public class WealthRecord
	{
		public int Value { get; private set; }
		public int Percents { get; private set; }
		public int ValuePerFertilityLevel { get; private set; }

		public double Calculate(int fertilityLevel)
		{
			return (Value + ValuePerFertilityLevel * fertilityLevel) * (1 + Percents * 0.01);
		}

		public void AddToValue(int value)
		{
			Value += value;
		}

		public void AddToPercents(int value)
		{
			Percents += value;
		}

		public void AddToValuePerFertilityLevel(int value)
		{
			ValuePerFertilityLevel += value;
		}
	}
}