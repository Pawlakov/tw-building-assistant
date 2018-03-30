using System;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using System.Collections.Generic;
namespace Technologies
{
	public class SimpleTechnologyTree : ITechnologyTree
	{
		// Stałe:
		//
		private const int _technologyLevelsCount = 5;
		//
		// Stan wewnętrzny:
		//
		private int DesiredTechnologyLevelIndex { get; set; } = -1;
		private readonly TechnologyLevel[] _levels;
		//
		// Interfejs wewnętrzny
		//
		internal SimpleTechnologyTree(XElement element)
		{
			IEnumerable<TechnologyLevel> temporary = from XElement levelElement in element.Elements() select new TechnologyLevel(this, levelElement);
			_levels = new TechnologyLevel[_technologyLevelsCount];
			_levels[0] = new TechnologyLevel(this);
			temporary.ToArray().CopyTo(_levels, 1);
			for (int whichLevel = 1; whichLevel < _technologyLevelsCount; ++whichLevel)
				_levels[whichLevel].Cumulate(_levels[whichLevel - 1]);
		}
		//
		// Pomocnicze:
		//
		private void OnDesiredTechnologyLevelChanging()
		{
			DesiredTechnologyChanging?.Invoke(this, EventArgs.Empty);
		}
		private void OnDesiredTechnologyLevelChanged()
		{
			DesiredTechnologyChanged?.Invoke(this, EventArgs.Empty);
		}
		//
		// Interfejs publiczny:
		//
		public event DesiredTechnologyLevelChangedHandler DesiredTechnologyChanged;
		public event DesiredTechnologyLevelChangingHandler DesiredTechnologyChanging;
		public void ChangeDesiredTechnologyLevel()
		{
			OnDesiredTechnologyLevelChanging();
			Console.WriteLine("Pick your desired technology level:");
			do
			{
				Console.Write("From 0 to {0}: ", _technologyLevelsCount - 1);
				try
				{
					DesiredTechnologyLevelIndex = Convert.ToInt32(Console.ReadLine(), CultureInfo.InvariantCulture);
				}
				catch (Exception)
				{
					DesiredTechnologyLevelIndex = -1;
				}
			} while (DesiredTechnologyLevelIndex < 0 || DesiredTechnologyLevelIndex > (_technologyLevelsCount - 1));
			OnDesiredTechnologyLevelChanged();
		}
		public bool IsLevelReasearched(TechnologyLevel level)
		{
			if (DesiredTechnologyLevelIndex < 0)
				throw new InvalidOperationException("Desired technology level is not set.");
			for (int whichLevel = 0; whichLevel < DesiredTechnologyLevelIndex + 1; ++whichLevel)
				if (level == _levels[whichLevel])
					return true;
			return false;
		}
		public TechnologyLevel Parse(int level, bool? isLegacy)
		{
			if (level < 0 || level >= _technologyLevelsCount)
				throw new ArgumentOutOfRangeException("level", level, "Technology level out of range.");
			if (isLegacy.HasValue)
				throw new ArgumentOutOfRangeException("useLegacy", isLegacy, "I can't come up with clear message for this one.");
			return _levels[level];
		}
	}
}