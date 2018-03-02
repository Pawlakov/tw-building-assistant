namespace TWAssistant
{
	namespace Attila
	{
		public class XorShift
		{
			uint x;
			uint y;
			uint z;
			public XorShift(uint seed)
			{
				x = 0x075bcd15 + seed;
				y = 0x159a55e5;
				z = 0x1f123bb5;
			}
			public uint Next(uint lowerLimit, uint upperLimit)
			{
				uint t;
				x ^= x << 16;
				x ^= x >> 5;
				x ^= x << 1;
				t = x;
				x = y;
				y = z;
				z = t ^ x ^ y;
				return (z % (upperLimit - lowerLimit)) + lowerLimit;
			}
		}
	}
}
