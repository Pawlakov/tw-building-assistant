namespace TWBuildingAssistant.Model.Technologies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using TWBuildingAssistant.Model.Religions;

    public class SimpleTechnologyTree : ITechnologyTree
    {
        private const int _technologyLevelsCount = 5;

        private int DesiredTechnologyLevelIndex { get; set; } = -1;

        private readonly TechnologyLevel[] _levels;

        public SimpleTechnologyTree(XElement element, IParser<IReligion> religionParser)
        {
            IEnumerable<TechnologyLevel> temporary = from XElement levelElement in element.Elements() select new TechnologyLevel(this, levelElement);
            _levels = new TechnologyLevel[_technologyLevelsCount];
            _levels[0] = new TechnologyLevel(this);
            temporary.ToArray().CopyTo(_levels, 1);
            for (int whichLevel = 1; whichLevel < _technologyLevelsCount; ++whichLevel)
                _levels[whichLevel].Cumulate(_levels[whichLevel - 1]);
            foreach (var level in this._levels)
            {
                foreach (var influence in level.Effect.Influences)
                {
                    influence.SetReligionParser(religionParser);
                }
            }
        }

        private void OnDesiredTechnologyLevelChanged()
        {
            DesiredTechnologyChanged?.Invoke(this, EventArgs.Empty);
        }

        public event DesiredTechnologyLevelChangedHandler DesiredTechnologyChanged;

        public void ChangeDesiredTechnologyLevel(int tier, bool useLegacy)
        {
            DesiredTechnologyLevelIndex = tier;
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

        public TechnologyLevel GetLevel(int level, bool? isLegacy)
        {
            if (level < 0 || level >= _technologyLevelsCount)
                throw new ArgumentOutOfRangeException("level", level, "Technology level out of range.");
            if (isLegacy.HasValue)
                throw new ArgumentOutOfRangeException("useLegacy", isLegacy, "I couldn't come up with clear message for this one.");
            return _levels[level];
        }

        public TechnologyLevel CurrentLevel
        {
            get
            {
                return GetLevel(DesiredTechnologyLevelIndex, null);
            }
        }
    }
}