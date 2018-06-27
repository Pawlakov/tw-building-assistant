using System;
using System.Collections.Generic;
namespace GameWorld.Religions
{
	// Klasa udaje prawdziwą religię i przekazuje wywołania do obecnej religii państwowej.
	public class StateReligion : IReligion
	{
		private Religion _currentStateReligion;
		//
		public StateReligion(ReligionsManager owner)
		{
			// Aktualizowanie odniesienia przy każdej zmianie religii państwowej.
			owner.StateReligionChanged += (Map.IStateReligionTracker sender, EventArgs e) => _currentStateReligion = sender.StateReligion;
		}
		public string Name
		{
			get
			{
				if (_currentStateReligion != null)
					return "State religion (" + _currentStateReligion.Name + ")";
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		public int PublicOrder
		{
			get
			{
				if (_currentStateReligion != null)
					return _currentStateReligion.PublicOrder;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		public int Growth
		{
			get
			{
				if (_currentStateReligion != null)
					return _currentStateReligion.Growth;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		public int ResearchRate
		{
			get
			{
				if (_currentStateReligion != null)
					return _currentStateReligion.ResearchRate;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		public int Sanitation
		{
			get
			{
				if (_currentStateReligion != null)
					return _currentStateReligion.Sanitation;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		public int ReligiousInfluence
		{
			get
			{
				if (_currentStateReligion != null)
					return _currentStateReligion.ReligiousInfluence;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		public int ReligiousOsmosis
		{
			get
			{
				if (_currentStateReligion != null)
					return _currentStateReligion.ReligiousOsmosis;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		public int Food
		{
			get
			{
				if (_currentStateReligion != null)
					return _currentStateReligion.Food;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		public int Fertility
		{
			get
			{
				if (_currentStateReligion != null)
					return _currentStateReligion.Fertility;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		public IEnumerable<WealthBonuses.WealthBonus> Bonuses
		{
			get
			{
				if (_currentStateReligion != null)
					return _currentStateReligion.Bonuses;
				throw new InvalidOperationException("State religion is not set yet.");
			}
		}
		public bool IsState
		{
			get { return true; }
		}
	}
}