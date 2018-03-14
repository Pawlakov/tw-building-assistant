using System;
using System.Xml;
namespace Faction
{
	class LegacyTechnologyTree : TechnologyTree
	{
		bool _useLegacy;
		readonly TechnologyLevel[] _antilegacyLevels;
		public LegacyTechnologyTree(XmlNodeList nodeList) :base(nodeList)
		{
			XmlNode temporaryNode;
			_antilegacyLevels = new TechnologyLevel[_technologyLevelsCount];
			for (int whichLevel = 0; whichLevel < _technologyLevelsCount; ++whichLevel)
			{
				temporaryNode = nodeList[whichLevel];
				if (temporaryNode.ChildNodes.Count != 2)
					throw new TechnologyException(String.Format("Niewłaściwa liczba węzłów XML w poziomie {0} technologii.", whichLevel));
				if (temporaryNode.ChildNodes[1].Name != "antilegacy")
					throw new TechnologyException(String.Format("Niewłaściwy węzeł XML w poziomie {0} technologii.", whichLevel));
				try
				{
					_antilegacyLevels[whichLevel] = new TechnologyLevel(temporaryNode.ChildNodes[1]);
				}
				catch (Exception exception)
				{
					throw new TechnologyException(String.Format("Nie powiodło się tworzenie {0} poziomu technolgii", whichLevel), exception);
				}
			}
			SumLevels();
			DecideOnUsingLegacy();
		}
		void SumLevels()
		{
			for (int whichLevel = 0; whichLevel < _technologyLevelsCount; ++whichLevel)
			{
				if (whichLevel != 0)
					_antilegacyLevels[whichLevel].Cumulate(_antilegacyLevels[whichLevel - 1]);
				_antilegacyLevels[whichLevel].Cumulate(_universalLevels[whichLevel]);
			}
		}
		public override TechnologyLevel DesiredTechnologyLevel
		{
			get
			{
				if (_useLegacy)
					return _universalLevels[_desiredTechnologyLevel];
				else
					return _antilegacyLevels[_desiredTechnologyLevel];
			}
		}
		void DecideOnUsingLegacy()
		{

		}
		public override string ToString()
		{
			
		}
	}
}