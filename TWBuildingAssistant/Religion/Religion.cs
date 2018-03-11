using System;
using System.Xml;
namespace Religion
{
	public class Religion
	{
		private const string _filename = "religions.xml";
		private static readonly Religion[] _religions;
		private static int _stateReligion;
		/// <summary>
		/// Liczba wszystkich dostępnych religii.
		/// </summary>
		public static int ReligionTypesCount
		{
			get { return _religions.Length; }
		}
		/// <summary>
		/// Statyczny konstruktor odpowiedzialny za przygotowanie wszystkich religii.
		/// </summary>
		static Religion()
		{
			XmlDocument sourceFile = new XmlDocument();
			try
			{
				sourceFile.Load(_filename);
			}
			catch (Exception exception)
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
				catch (Exception exception)
				{
					throw new ReligionException(String.Format("Nie powiodło się utworzenie obiektu religii (na miejscu {0} w pliku).", whichReligion), exception);
				}
			}
			_stateReligion = -1;
			PickStateReligion();
		}
		public static void PickStateReligion()
		{
			for (int whichReligion = 0; whichReligion < ReligionTypesCount; ++whichReligion)
				Console.WriteLine("{0}. {1}", whichReligion, _religions[whichReligion].ToString());
			while (true)
			{
				Console.Write("Pick your state religion: ");
				try
				{
					_stateReligion = Convert.ToInt32(Console.ReadLine());
				}
				catch (Exception)
				{
					_stateReligion = -1;
				}
				if (_stateReligion > -1 && _stateReligion < ReligionTypesCount)
					break;
			}
		}
		/// <summary>
		/// Weryfikuje czy dany tekst jest nazwą któreś religii.
		/// </summary>
		/// <param name="input">Tekst do weryfikacji.</param>
		/// <returns>Referencja do właściwego obiektu religii lub <see langword="null"/>.</returns>
		public static Religion Parse(string input)
		{
			if (input == "STATE")
				return StateReligion;
			foreach (Religion religion in _religions)
			{
				if (religion.Name == input)
					return religion;
			}
			return null;
		}
		/// <summary>
		/// Operator konwersji do liczby całkowitej.
		/// </summary>
		/// <param name="religion">Konwertowana wartość.</param>
		public static explicit operator int(Religion religion)
		{
			for (int whichReligion = 0; whichReligion < _religions.Length; ++whichReligion)
				if (religion == _religions[whichReligion])
					return whichReligion;
			throw new ReligionException(String.Format("Nieznany obiekt religii (nazwa: {0}). Skąd go masz?", religion.Name));
		}
		/// <summary>
		/// Religia wybrana przez użytkownika jako państwowa.
		/// </summary>
		public static Religion StateReligion
		{
			get { return _religions[_stateReligion]; }
		}
		//
		//
		//
		/// <summary>
		/// Tworzy nową religię na podstawie danych z XML.
		/// </summary>
		/// <param name="node">Węzeł XML zawierający niezbędne informacje.</param>
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
		/// <summary>
		/// Nazwa religii.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// Bonus do porządku publicznego zapewniany przez tą religię.
		/// </summary>
		public int PublicOrder { get; }
		/// <summary>
		/// Bonus do wzrostu zapewniany przez tą religię.
		/// </summary>
		public int Growth { get; }
		/// <summary>
		/// Bonus do prędkości badań zapewniany przez tą religię.
		/// </summary>
		public int ResearchRate { get; }
		/// <summary>
		/// Bonus do higieny zapewniany przez tą religię.
		/// </summary>
		public int Sanitation { get; }
		/// <summary>
		/// Bonus do wpływów religijnych zapewniany przez tą religię (wewnątrz prowincji).
		/// </summary>
		public int ReligiousInfluence { get; }
		/// <summary>
		/// Bonus do wpływów religijnych zapewniany przez tą religię (do prowincji sąsiednich).
		/// </summary>
		public int ReligiousOsmosis { get; }
		/// <summary>
		/// Bonusy dochodowe zapewniane przez tą religię.
		/// </summary>
		public Wealth.WealthBonus[] WealthBonuses { get; }
		/// <summary>
		/// Zwraca krótki tekstowy opis tego obiektu.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string result = Name;
			if (WealthBonuses.Length > 0)
				foreach (Wealth.WealthBonus bonus in WealthBonuses)
					result += bonus.ToString();
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
		/// <summary>
		/// Sprawdza czy ta religia jest religią państwową.
		/// </summary>
		/// <returns>Wynik sprawdzenia.</returns>
		public bool IsState()
		{
			return this == StateReligion;
		}
	}

	public class ReligionException : Exception
	{
		public ReligionException() : base("Bład w module religii.") { }
		public ReligionException(string message) : base(message) { }
		public ReligionException(string message, Exception innerException) : base(message, innerException) { }
	}
}