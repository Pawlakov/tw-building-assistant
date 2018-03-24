using System;
using System.Xml;
using System.Globalization;
namespace Religions
{
	/// <summary>
	/// Obiekt tej klasy stanowi opis jednej religii.
	/// </summary>
	public class Religion : IReligion
	{
		// Interfejs wewnętrzny:
		//
		internal Religion(XmlNode node)
		{
			if (node.Name != "religion")
				throw new ArgumentException("Given node is not religion node.", "node");
			XmlAttributeCollection attributes = node.Attributes;
			XmlNodeList children = node.ChildNodes;
			if (children.Count > 1)
				throw new ArgumentException("XML node has too many child elements.", "node");
			XmlNode temporary = attributes.GetNamedItem("n");
			if (temporary == null)
				throw new FormatException("Could not read the 'n' attribute of XML node.");
			Name = temporary.InnerText;
			try
			{
				temporary = attributes.GetNamedItem("po");
				if (temporary != null)
					PublicOrder = Convert.ToInt32(temporary.InnerText, CultureInfo.InvariantCulture);
				temporary = attributes.GetNamedItem("g");
				if (temporary != null)
					Growth = Convert.ToInt32(temporary.InnerText, CultureInfo.InvariantCulture);
				temporary = attributes.GetNamedItem("rr");
				if (temporary != null)
					ResearchRate = Convert.ToInt32(temporary.InnerText, CultureInfo.InvariantCulture);
				temporary = attributes.GetNamedItem("s");
				if (temporary != null)
					Sanitation = Convert.ToInt32(temporary.InnerText, CultureInfo.InvariantCulture);
				temporary = attributes.GetNamedItem("ri");
				if (temporary != null)
					ReligiousInfluence = Convert.ToInt32(temporary.InnerText, CultureInfo.InvariantCulture);
				temporary = attributes.GetNamedItem("ro");
				if (temporary != null)
					ReligiousOsmosis = Convert.ToInt32(temporary.InnerText, CultureInfo.InvariantCulture);
			}
			catch (Exception exception)
			{
				throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Failed to read attributes of religion {0}.", Name), exception);
			}
			try
			{
				WealthBonus = WealthBonuses.BonusFactory.MakeBonus(children[0]);
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
