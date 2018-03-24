using System;
using System.Globalization;
namespace Religions
{
	/// <summary>
	/// Obiekt tej klasy przekazuje wywołania do obecnego obiektu religii państwowej.
	/// </summary>
	public class StateReligion : IReligion
	{
		// Interfejs wewnętrzny:
		//
		internal StateReligion()
		{
			ReligionsManager.Singleton.StateReligionChanged += (ReligionsManager sender, EventArgs e) => CurrentStateReligion = sender.StateReligion;
		}
		//
		// Stan wewnętrzny:
		//
		private Religion CurrentStateReligion { get; set; }
		//
		// Interfejs publiczny:
		//
		public string Name
		{
			get
			{
				return String.Format(CultureInfo.CurrentCulture, "State religion ({0})", CurrentStateReligion.Name);
			}
		}
		/// <summary>
		/// Bonus do porządku publicznego zapewniany przez tą religię.
		/// </summary>
		public int PublicOrder
		{
			get
			{
				if (CurrentStateReligion != null)
					return CurrentStateReligion.PublicOrder;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		/// <summary>
		/// Bonus do wzrostu zapewniany przez tą religię.
		/// </summary>
		public int Growth
		{
			get
			{
				if (CurrentStateReligion != null)
					return CurrentStateReligion.Growth;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		/// <summary>
		/// Bonus do prędkości badań zapewniany przez tą religię.
		/// </summary>
		public int ResearchRate
		{
			get
			{
				if (CurrentStateReligion != null)
					return CurrentStateReligion.ResearchRate;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		/// <summary>
		/// Bonus do higieny zapewniany przez tą religię.
		/// </summary>
		public int Sanitation
		{
			get
			{
				if (CurrentStateReligion != null)
					return CurrentStateReligion.Sanitation;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		/// <summary>
		/// Bonus do wpływów religijnych zapewniany przez tą religię (wewnątrz prowincji).
		/// </summary>
		public int ReligiousInfluence
		{
			get
			{
				if (CurrentStateReligion != null)
					return CurrentStateReligion.ReligiousInfluence;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		/// <summary>
		/// Bonus do wpływów religijnych zapewniany przez tą religię (do prowincji sąsiednich).
		/// </summary>
		public int ReligiousOsmosis
		{
			get
			{
				if (CurrentStateReligion != null)
					return CurrentStateReligion.ReligiousOsmosis;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		/// <summary>
		/// Bonusy dochodowe zapewniane przez tą religię.
		/// </summary>
		public WealthBonuses.WealthBonus WealthBonus
		{
			get
			{
				if (CurrentStateReligion != null)
					return CurrentStateReligion.WealthBonus;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		public override string ToString()
		{
			string result = Name;
			if (WealthBonus != null)
				result += WealthBonus.ToString();
			if (PublicOrder != 0)
				result += (" public order +" + PublicOrder);
			if (Growth != 0)
				result += (" growth +" + Growth);
			if (ResearchRate != 0)
				result += (" research rate +" + ResearchRate + "%");
			if (Sanitation != 0)
				result += (" sanitation +" + Sanitation);
			if (ReligiousInfluence != 0)
				result += (" religious influence +" + ReligiousInfluence);
			if (ReligiousOsmosis != 0)
				result += (" religious osmosis +" + ReligiousOsmosis);
			return result;
		}
	}
}
