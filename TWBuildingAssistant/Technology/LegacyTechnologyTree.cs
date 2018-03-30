using System;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using System.Collections.Generic;
namespace Technologies
{
	public class LegacyTechnologyTree : ITechnologyTree
	{
		// Stałe:
		//
		private const int _technologyLevelsCount = 4; // Bez korzenia
													  //
													  // Stan wewnętrzny:
													  //
		private bool UseLegacy { get; set; }
		private int DesiredTechnologyLevelIndex { get; set; }
		private readonly TechnologyLevel[] _universalLevels; // Pamiętaj: Indeksowane od jedynki!!!
		private readonly TechnologyLevel[] _antilegacyLevels;
		private readonly TechnologyLevel[] _legacyLevels;
		private readonly TechnologyLevel _rootLevel;
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
				Console.Write("From 0 to {0}: ", _technologyLevelsCount);
				try
				{
					DesiredTechnologyLevelIndex = Convert.ToInt32(Console.ReadLine(), CultureInfo.InvariantCulture);
				}
				catch (Exception)
				{
					DesiredTechnologyLevelIndex = -1;
				}
			} while (DesiredTechnologyLevelIndex < 0 || DesiredTechnologyLevelIndex > _technologyLevelsCount);
			Console.WriteLine("Use legacy technologies?");
			while (true)
			{
				Console.Write("True or false: ");
				try
				{
					UseLegacy = Convert.ToBoolean(Console.ReadLine());
					break;
				}
				catch (Exception) { }
			}
			OnDesiredTechnologyLevelChanged();
		}
		public bool IsLevelReasearched(TechnologyLevel level)
		{
			if (DesiredTechnologyLevelIndex < 0)
				throw new InvalidOperationException("Desired technology level is not set.");
			if (level == _rootLevel)
				return true;
			for (int whichLevel = 0; whichLevel < _technologyLevelsCount; ++whichLevel)
			{
				if (level == _universalLevels[whichLevel])
				{
					if (whichLevel + 1 <= DesiredTechnologyLevelIndex)
						return true;
					return false;
				}
				if (level == _antilegacyLevels[whichLevel])
				{
					if (UseLegacy)
						return false;
					else
					{
						if (whichLevel + 1 <= DesiredTechnologyLevelIndex)
							return true;
						return false;
					}
				}
				if (level == _legacyLevels[whichLevel])
				{
					if (UseLegacy)
						return true;
					else
					{
						if (whichLevel + 1 <= DesiredTechnologyLevelIndex)
							return false;
						return true;
					}
				}
			}
			return false;
		}
		public TechnologyLevel Parse(int level, bool? isLegacy)
		{
			if (level < 0 || level >= _technologyLevelsCount + 1)
				throw new ArgumentOutOfRangeException("level", level, "Technology level out of range.");
			if (level == 0)
				return _rootLevel;
			if (!isLegacy.HasValue)
				return _universalLevels[level - 1];
			if (isLegacy.Value)
				return _legacyLevels[level - 1];
			return _antilegacyLevels[level - 1];
		}
		//
		// Interfejs wewnętrzny:
		//
		internal LegacyTechnologyTree(XElement element)
		{
			_rootLevel = new TechnologyLevel(this);
			IEnumerable<XElement> tierElements = from tierElement in element.Elements() select tierElement;
			IEnumerable<XElement> universalElements = from tierElement in tierElements select tierElement.Element("universal");
			IEnumerable<XElement> antilegacyElements = from tierElement in tierElements select tierElement.Element("antilegacy");
			_universalLevels = (from levelElement in universalElements select new TechnologyLevel(this, levelElement)).ToArray();
			_antilegacyLevels = (from levelElement in antilegacyElements select new TechnologyLevel(this, levelElement)).ToArray();
			_legacyLevels = (from levelElement in antilegacyElements select new TechnologyLevel(this)).ToArray();
			//
			for (int whichLevel = 0; whichLevel < _technologyLevelsCount - 1; ++whichLevel)
			{
				_antilegacyLevels[whichLevel].Cumulate(_universalLevels[whichLevel]);
				_legacyLevels[whichLevel].Cumulate(_universalLevels[whichLevel]);
			}
			for (int whichLevel = 1; whichLevel < _technologyLevelsCount - 1; ++whichLevel)
			{
				_universalLevels[whichLevel].Cumulate(_universalLevels[whichLevel - 1]);
				_antilegacyLevels[whichLevel].Cumulate(_antilegacyLevels[whichLevel - 1]);
				_legacyLevels[whichLevel].Cumulate(_legacyLevels[whichLevel - 1]);
			}
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
	} // Stworzyć tylko jeden poziom zerowy.
}