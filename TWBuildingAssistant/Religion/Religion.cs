using System;
using System.Xml;
namespace Religion
{
	public class Religion
	{
		private const string filename = "religions.xml";
		private static readonly Religion[] _religions;
		private static readonly int _stateReligion;
		public static int ReligionTypesCount
		{
			get { return _religions.Length; }
		}
		static Religion()
		{
			XmlDocument sourceFile = new XmlDocument();
			try
			{
				sourceFile.Load(filename);
			}
			catch(Exception exception)
			{
				throw new ReligionException("Nie powiodło się otwarcie pliku opisującego religie.", exception);
			}
			XmlNodeList nodeList = sourceFile.GetElementsByTagName("religion");
			_religions = new Religion[nodeList.Count];
			for (int whichReligion = 0; whichReligion < _religions.Length; ++whichReligion)
			{
				try
				{
					_religions[whichReligion] = new Religion(nodeList[whichReligion]);
				}
				catch(Exception exception)
				{
					throw new ReligionException(String.Format("Nie powiodło się utworzenie obiektu religii (na miejscu {0} w pliku).", whichReligion), exception);
				}
			}
			//StateReligionDialog dialog = new StateReligionDialog();
		}
		public static Religion Parse(string input)
		{
			foreach (Religion religion in _religions)
			{
				if (religion.Name == input)
					return religion;
			}
			return null;
		}
		public static explicit operator int(Religion religion)
		{
			for (int whichReligion = 0; whichReligion < _religions.Length; ++whichReligion)
				if (religion == _religions[whichReligion])
					return whichReligion;
			throw new ReligionException("Nieznany obiekt klasy Religion. Skąd go masz?");
		}
		public static Religion StateReligion
		{
			get { return _religions[_stateReligion]; }
		}
		//
		//
		//
		private Religion(XmlNode node)
		{
			PublicOrder = 0;
			Growth = 0;
			ResearchRate = 0;
			Sanitation = 0;
			ReligiousInfluence = 0;
			ReligiousOsmosis = 0;
			//
			if (node == null)
				throw new ReligionException("Próbowano utworzyć religię na podstawie pustej wartości.");
			XmlNode temporary = node.Attributes.GetNamedItem("n");
			if (temporary == null)
				throw new ReligionException("Próbowano utworzyć religię bez nazwy.");
			Name = temporary.InnerText;
			//
			temporary = node.Attributes.GetNamedItem("po");
			if (temporary != null)
				PublicOrder = Convert.ToInt32(temporary.InnerText);
			temporary = node.Attributes.GetNamedItem("g");
			if (temporary != null)
				Growth = Convert.ToInt32(temporary.InnerText);
			temporary = node.Attributes.GetNamedItem("rr");
			if (temporary != null)
				ResearchRate = Convert.ToInt32(temporary.InnerText);
			temporary = node.Attributes.GetNamedItem("s");
			if (temporary != null)
				Sanitation = Convert.ToInt32(temporary.InnerText);
			temporary = node.Attributes.GetNamedItem("ri");
			if (temporary != null)
				ReligiousInfluence = Convert.ToInt32(temporary.InnerText);
			temporary = node.Attributes.GetNamedItem("ro");
			if (temporary != null)
				ReligiousOsmosis = Convert.ToInt32(temporary.InnerText);
			//
			try
			{
				XmlNodeList bonusNodeList = node.ChildNodes;
				WealthBonuses = new Wealth.WealthBonus[bonusNodeList.Count];
				for (int whichBonus = 0; whichBonus < WealthBonuses.Length; ++whichBonus)
				{
					WealthBonuses[whichBonus] = Wealth.WealthBonus.MakeBonus(bonusNodeList.Item(whichBonus));
				}
			}
			catch (Exception exception)
			{
				throw new ReligionException(String.Format("Nie powiodło się tworzenie obiektu bonusu dochodowego dla obiektu religii {0}.", Name), exception);
			}
		}
		public string Name { get; }
		public int PublicOrder { get; }
		public int Growth { get; }
		public int ResearchRate { get; }
		public int Sanitation { get; }
		public int ReligiousInfluence { get; }
		public int ReligiousOsmosis { get; }
		public Wealth.WealthBonus[] WealthBonuses { get; }
		public override string ToString()
		{
			string result = Name;
			if (WealthBonuses.Length > 0)
				foreach (Wealth.WealthBonus bonus in WealthBonuses)
					result += bonus.ToString();
			if (PublicOrder != 0)
				result += (" public order +" + PublicOrder);
			if (PublicOrder != 0)
				result += (" growth +" + Growth);
			if (PublicOrder != 0)
				result += (" research rate +" + ResearchRate + "%");
			if (PublicOrder != 0)
				result += (" sanitation +" + Sanitation);
			if (PublicOrder != 0)
				result += (" religious influence +" + ReligiousInfluence);
			if (PublicOrder != 0)
				result += (" religious osmosis +" + ReligiousOsmosis);
			return result;
		}
	}

	public class ReligionException : Exception
	{
		public ReligionException() : base("Bład w module religii.") { }
		public ReligionException(string message) : base(message) { }
		public ReligionException(string message, Exception innerException) : base(message, innerException) { }
	}
}