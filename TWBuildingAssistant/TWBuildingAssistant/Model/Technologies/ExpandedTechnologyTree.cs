namespace TWBuildingAssistant.Model.Technologies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class ExpandedTechnologyTree : ITechnologyTree
    {
        private const int _technologyLevelsCount = 4;

        private bool UseLegacy { get; set; }

        private int DesiredTechnologyLevelIndex { get; set; } = -1;

        private readonly TechnologyLevel[] _universalLevels;

        private readonly TechnologyLevel[] _antilegacyLevels;

        private readonly TechnologyLevel[] _legacyLevels;

        private readonly TechnologyLevel _rootLevel;

        public event DesiredTechnologyLevelChangedHandler DesiredTechnologyChanged;

        public void ChangeDesiredTechnologyLevel(int tier, bool useLegacy)
        {
            DesiredTechnologyLevelIndex = tier;
            UseLegacy = useLegacy;
            OnDesiredTechnologyLevelChanged();
        }

        public bool IsLevelReasearched(TechnologyLevel level)
        {
            if (DesiredTechnologyLevelIndex < 0)
                throw new InvalidOperationException("Desired technology level is not set.");
            //
            if (level == _rootLevel)
                return true;
            // Tu następuje właściwe sprawdzenie warunku "Czy poziom jest dostępny?".
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

        public TechnologyLevel GetLevel(int level, bool? isLegacy)
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

        public TechnologyLevel CurrentLevel
        {
            get
            {
                return GetLevel(DesiredTechnologyLevelIndex, UseLegacy);
            }
        }

        public ExpandedTechnologyTree(XElement element)
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

        private void OnDesiredTechnologyLevelChanged()
        {
            DesiredTechnologyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
