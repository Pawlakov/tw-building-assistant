using System;
using System.Xml;
namespace Technology
{
	class TechnologyTree
	{
		protected int _desiredTechnologyLevel;
		protected const int _technologyLevelsCount = 5;
		protected readonly TechnologyLevel[] _universalLevels;
		public TechnologyTree(XmlNodeList nodeList)
		{
			if (nodeList == null)
				throw new TechnologyException("Próbowano utworzyć drzewo technologii na podstawie pustej wartości.");
			if (nodeList.Count != _technologyLevelsCount)
				throw new TechnologyException("Prze stosupełnienie. A tak na serio, to zła liczba poziomów technologi w pliku XML.");
			XmlNode temporaryNode;
			_universalLevels = new TechnologyLevel[_technologyLevelsCount];
			for (int whichLevel = 0; whichLevel < _technologyLevelsCount; ++whichLevel)
			{
				temporaryNode = nodeList[whichLevel];
				if (temporaryNode.ChildNodes.Count != 1 && temporaryNode.ChildNodes.Count != 2)
					throw new TechnologyException(String.Format("Niewłaściwa liczba węzłów XML w poziomie {0} technologii", whichLevel));
				if (temporaryNode.ChildNodes[0].Name != "universal")
					throw new TechnologyException(String.Format("Niewłaściwy węzeł XML w poziomie {0} technologii.", whichLevel));
				try
				{
					_universalLevels[whichLevel] = new TechnologyLevel(temporaryNode.ChildNodes[0]);
				}
				catch (Exception exception)
				{
					throw new TechnologyException(String.Format("Nie powiodło się tworzenie {0} poziomu technolgii.", whichLevel), exception);
				}
			}
			SumLevels();
			PickDesiredTechnologyLevel();
		}
		void SumLevels()
		{
			for (int whichLevel = 1; whichLevel < _technologyLevelsCount; ++whichLevel)
				_universalLevels[whichLevel].Cumulate(_universalLevels[whichLevel - 1]);
		}
		void PickDesiredTechnologyLevel()
		{

		}
		public virtual TechnologyLevel DesiredTechnologyLevel
		{
			get
			{
				return _universalLevels[_desiredTechnologyLevel];
			}
		}
		public override string ToString()
		{
			
		}
	}

	class TechnologyException : Exception
	{
		public TechnologyException() : base("Bład w module technologii.") { }
		public TechnologyException(string message) : base(message) { }
		public TechnologyException(string message, Exception innerException) : base(message, innerException) { }
	}
}