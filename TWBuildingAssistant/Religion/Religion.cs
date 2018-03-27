using System;
using System.Linq;
using System.Xml.Linq;
namespace Religions
{
	/// <summary>
	/// Obiekt tej klasy stanowi opis jednej religii.
	/// </summary>
	public class Religion : IReligion
	{
		// Interfejs wewnętrzny:
		//
		internal Religion(XElement element)
		{
			Name = (string)element.Attribute("n");
			int? temporary = (int?)element.Attribute("po");
			if (temporary.HasValue)
				PublicOrder = temporary.Value;
			temporary = (int?)element.Attribute("g");
			if (temporary.HasValue)
				Growth = temporary.Value;
			temporary = (int?)element.Attribute("rr");
			if (temporary.HasValue)
				ResearchRate = temporary.Value;
			temporary = (int?)element.Attribute("s");
			if (temporary.HasValue)
				Sanitation = temporary.Value;
			temporary = (int?)element.Attribute("ri");
			if (temporary.HasValue)
				ReligiousInfluence = temporary.Value;
			temporary = (int?)element.Attribute("ro");
			if (temporary.HasValue)
				ReligiousOsmosis = temporary.Value;
			try
			{
				WealthBonus = WealthBonuses.BonusFactory.MakeBonus(element.Elements().ToArray()[0]);
			}
			catch (Exception exception)
			{
				throw new Exception(String.Format("Could not create wealth bonus object for religion {0}.", Name), exception);
			}
		}
		//
		// Interfejs publiczny:
		//
		/// <summary>
		/// Nazwa religii.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// Bonus do porządku publicznego zapewniany przez tą religię.
		/// </summary>
		public virtual int PublicOrder { get; }
		/// <summary>
		/// Bonus do wzrostu zapewniany przez tą religię.
		/// </summary>
		public virtual int Growth { get; }
		/// <summary>
		/// Bonus do prędkości badań zapewniany przez tą religię.
		/// </summary>
		public virtual int ResearchRate { get; }
		/// <summary>
		/// Bonus do higieny zapewniany przez tą religię.
		/// </summary>
		public virtual int Sanitation { get; }
		/// <summary>
		/// Bonus do wpływów religijnych zapewniany przez tą religię (wewnątrz prowincji).
		/// </summary>
		public virtual int ReligiousInfluence { get; }
		/// <summary>
		/// Bonus do wpływów religijnych zapewniany przez tą religię (do prowincji sąsiednich).
		/// </summary>
		public virtual int ReligiousOsmosis { get; }
		/// <summary>
		/// Bonusy dochodowe zapewniane przez tą religię.
		/// </summary>
		public virtual WealthBonuses.WealthBonus WealthBonus { get; }
		/// <summary>
		/// Zwraca krótki tekstowy opis tego obiektu.
		/// </summary>
		/// <returns></returns>
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
