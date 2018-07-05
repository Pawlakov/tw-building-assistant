using System;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
namespace GameWorld.Religions
{
	// Klasa do zarządzania wszystkimi religiami.
	public class ReligionsManager : Map.IReligionParser, Map.IStateReligionTracker
	{
		public ReligionsManager()
		{
			if (!Validate(out string message))
				throw new FormatException("Cannot create information on religions: " + message);
			XDocument sourceFile = XDocument.Load(_sourceFile);
			_religions = (from XElement element in sourceFile.Root.Elements() select new Religion(element, this)).ToArray();
			StateReligionProxy = new StateReligion(this);
		}
		// Wydarzenie zmiany religii państwowej.
		public event Map.StateReligionChangedHandler StateReligionChanged;
		public int ReligionTypesCount
		{
			get { return _religions.Length; }
		}
		// Przekłada nazwę religii na odpowiedni obiekt.
		public IReligion Parse(string input)
		{
			if (input == null)
				return null;
			// Zwrócenie obiektu zawsze będącego religią państwową.
			if (input.Equals("State", StringComparison.OrdinalIgnoreCase))
				return StateReligionProxy;
			// Lub obiektu prawdziwej religii.
			foreach (Religion religion in _religions)
				if (input.Equals(religion.Name, StringComparison.OrdinalIgnoreCase))
					return religion;
			return null;
		}
		public int GetIndex(IReligion religion)
		{
			if (religion == null)
				throw new ArgumentNullException("religion");
			if (religion == StateReligionProxy)
				return StateReligionIndex;
			for (int whichReligion = 0; whichReligion < ReligionTypesCount; ++whichReligion)
				if (religion == _religions[whichReligion])
					return whichReligion;
			throw new ArgumentException("Unknown religion (name: " + religion.Name + ").", "religion");
		}
		public Religion StateReligion
		{
			get { return _religions[StateReligionIndex]; }
		}
		// Zmienia religię państwową na tą o podanym indeksie.
		public void ChangeStateReligion(int whichReligion)
		{
			if (whichReligion < 0 || whichReligion > (ReligionTypesCount - 1))
				throw new ArgumentOutOfRangeException("whichReligion", whichReligion, "The index of new state religion is out of range.");
			StateReligionIndex = whichReligion;
			OnStateReligionChanged();
		}
		// Sprawdzenie czy dany obiekt reprezentuje religię państwową.
		public bool IsStateReligion(IReligion subject)
		{
			if (subject == StateReligionProxy)
				return true;
			return subject == StateReligion;
		}
		// Zwraca nazwy wszystkich religii z ich indeksami.
		public IEnumerable<KeyValuePair<int, string>> AllReligionsNames
		{
			get
			{
				List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>(ReligionTypesCount);
				for (int whichReligion = 0; whichReligion < ReligionTypesCount; ++whichReligion)
					result.Add(new KeyValuePair<int, string>(whichReligion, _religions[whichReligion].Name));
				return result;
			}
		}
		//
		private const string _sourceFile = "Religions\\twa_religions.xml";
		//
		private readonly Religion[] _religions;
		// Pośrednik zawsze będący religią państwową.
		private StateReligion StateReligionProxy { get; }
		// Indeks aktualnej religii państwowej.
		private int StateReligionIndex { get; set; } = -1;
		//
		private void OnStateReligionChanged()
		{
			StateReligionChanged?.Invoke(this, EventArgs.Empty);
		}
		//
		public static bool Validate(out string message)
		{
			XDocument document;
			if (!File.Exists(_sourceFile))
			{
				message = "Corresponding file not found.";
				return false;
			}
			document = XDocument.Load(_sourceFile);
			if (document.Root == null || document.Root.Elements().Count() < 1)
			{
				message = "Corresponding file is incomplete.";
				return false;
			}
			foreach (XElement element in document.Root.Elements())
				if (!Religion.ValidateElement(element, out string elementMessage))
				{
					message = "One of XML elements is invalid: " + elementMessage;
					return false;
				}
			message = "Information on religions is valid and complete.";
			return true;
		}
		//
		public int PublicOrder
		{
			get { return StateReligion.PublicOrder; }
		}
		public int Food
		{
			get { return StateReligion.Food; }
		}
		public int Sanitation
		{
			get { return StateReligion.Sanitation; }
		}
		public int ReligiousOsmosis
		{
			get { return StateReligion.ReligiousOsmosis; }
		}
		public int ReligiousInfluence
		{
			get { return StateReligion.ReligiousInfluence; }
		}
		public int ResearchRate
		{
			get { return StateReligion.ResearchRate; }
		}
		public int Growth
		{
			get { return StateReligion.Growth; }
		}
		public int Fertility
		{
			get { return StateReligion.Fertility; }
		}
		public IEnumerable<Effects.WealthBonus> Bonuses
		{
			get { return StateReligion.Bonuses; }
		}
	}
}
