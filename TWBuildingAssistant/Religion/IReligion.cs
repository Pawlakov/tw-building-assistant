namespace Religions
{
	/// <summary>
	/// Interfejs obiektu mogącego reprezentować religię.
	/// </summary>
	public interface IReligion
	{
		/// <summary>
		/// Nazwa religii.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// Bonus do porządku publicznego zapewniany przez tą religię.
		/// </summary>
		int PublicOrder { get; }
		/// <summary>
		/// Bonus do wzrostu zapewniany przez tą religię.
		/// </summary>
		int Growth { get; }
		/// <summary>
		/// Bonus do prędkości badań zapewniany przez tą religię.
		/// </summary>
		int ResearchRate { get; }
		/// <summary>
		/// Bonus do higieny zapewniany przez tą religię.
		/// </summary>
		int Sanitation { get; }
		/// <summary>
		/// Bonus do wpływów religijnych zapewniany przez tą religię (wewnątrz prowincji).
		/// </summary>
		int ReligiousInfluence { get; }
		/// <summary>
		/// Bonus do wpływów religijnych zapewniany przez tą religię (do prowincji sąsiednich).
		/// </summary>
		int ReligiousOsmosis { get; }
		/// <summary>
		/// Bonusy dochodowe zapewniane przez tą religię.
		/// </summary>
		WealthBonuses.WealthBonus WealthBonus { get; }
	}
}
