using System;
namespace Religion
{
	public class ProvinceReligion
	{
		int _influence;
		int _counterinfluence;
		Religion _stateReligion;
		//
		public ProvinceReligion(ProvinceTraditions localTraditions, Religion stateReligion)
		{
			_stateReligion = stateReligion;
			_influence = localTraditions.GetTraditionExactly(_stateReligion) + Globals.EnvInfluence;
			_counterinfluence = localTraditions.GetTraditionExcept(_stateReligion);
		}
		public void AddInfluence(int ammount, Religion religion)
		{
			if (this.religion == religion || religion == Religion.STATE)
				influence += ammount;
			else
				counterinfluence += ammount;
		}
		public int Order
		{
			get
			{
				float percentage = StateReligionPercentage;
				return (int)Math.Floor(-6 + (percentage * 17 + 220) / 300);
			}
		}
		public int Influence
		{
			get { return _influence; }
		}
		public int Counterinfluence
		{
			get { return _counterinfluence; }
		}
		public double StateReligionPercentage
		{
			get
			{
				return 100.0 * (_influence / (_counterinfluence + (double)_influence));
			}
		}
	}
}
