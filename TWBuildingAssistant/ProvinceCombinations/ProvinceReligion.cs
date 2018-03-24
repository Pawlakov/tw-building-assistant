using System;
namespace Religions
{
	/// <summary>
	/// Obiekty tej zawiera informacje o wplywach religijnych wewnątrz jednej prowincji.
	/// </summary>
	public class ProvinceReligion
	{
		/// <summary>
		/// Tworzy nowy obiekt informacji o religii wewnątrz prowincji.
		/// </summary>
		/// <param name="localTraditions">Zbiór tradycji religijnych prowincji dla której tworzony jest ten obiekt.</param>
		public ProvinceReligion(ProvinceTraditions localTraditions)
		{
			if (localTraditions == null)
				throw new ReligionException("Próbowano utworzyć obiekt religii prowincji bez podania jej zbioru tradycji religijnych.");
			if (Religion.StateReligion == null)
				throw new ReligionException("Próbowano utworzyć obiekt religii prowincji nie mając wybranej religii państwowej.");
			Influence = localTraditions.GetTraditionExactly(Religion.StateReligion) + Religion.StateReligion.ReligiousInfluence;
			Counterinfluence = localTraditions.GetTraditionExcept(Religion.StateReligion);
		}
		/// <summary>
		/// Funkcja dodaje określoną liczbę wpływów pewnej religii.
		/// </summary>
		/// <param name="ammount">Ilosć dodanych wpływów.</param>
		/// <param name="religion">Której religi wływy będą dodane.</param>
		public void AddInfluence(int ammount, Religion religion)
		{
			if (religion.IsState())
				Influence += ammount;
			else
				Counterinfluence += ammount;
		}
		/// <summary>
		/// Wpływ obecnego rozkładu religii w prowincji na porządek publiczny.
		/// </summary>
		public int PublicOrder
		{
			get
			{
				throw new NotImplementedException("Wątpliwej jakości wzór na obliczanie wpływu religii na porządek publiczny.");
				//double percentage = StateReligionPercentage;
				//return (int)Math.Floor(-6 + (percentage * 17 + 220) / 300);
			}
		}
		/// <summary>
		/// Wpływ (w procentach) religii państwowej w prowincji.
		/// </summary>
		public double StateReligionPercentage
		{
			get
			{
				return 100.0 * (Influence / (Counterinfluence + (double)Influence));
			}
		}
		//
		// Stan wewnętrzny:
		//
		private readonly int[] _influences;
		private Religion CurrentStateReligion { get; set; }
	}
}
