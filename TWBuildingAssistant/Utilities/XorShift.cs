using System;
namespace Utilities
{
	/// <summary>
	/// Szybki generator liczb losowych.
	/// </summary>
	public static class XorShift
	{
		private const int _startingX = -2024026859;
		private const int _startingY = -1785047579;
		private const int _startingZ = -1626195019;
		private static int _x;
		private static int _y;
		private static int _z;
		static XorShift()
		{
			DateTime currentTime = DateTime.Now;
			_x = currentTime.Millisecond + _startingX;
			_y = _startingY;
			_z = _startingZ;
		}
		/// <summary>
		/// Zwraca kolejną losową liczbę całkowitą.
		/// </summary>
		/// <param name="lowerLimit">Dolna granica przedziału losowania. Wylosowana liczba nie będzie od niej mniejsza.</param>
		/// <param name="upperLimit">Górna granica przedziału losowania. Wylosowana liczba będzie od niej mniejsza.</param>
		/// <returns>Wartość (pseudo)losowa.</returns>
		public static int Next(int lowerLimit, int upperLimit)
		{
			int temporary;
			_x ^= (_x << 16);
			_x ^= (_x >> 5);
			_x ^= (_x << 1);
			temporary = _x;
			_x = _y;
			_y = _z;
			_z = temporary ^ _x ^ _y;
			if(_z < 0)
				return -(_z + 1) % (upperLimit - lowerLimit) + lowerLimit;
			else
				return _z % (upperLimit - lowerLimit) + lowerLimit;
		}
	}
}