namespace TWBuildingAssistant.Model.Technologies
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    using TWBuildingAssistant.Model.Religions;

    public class ExpandedTechnologyTree : ITechnologyTree
    {
        private const int TechnologyLevelsCount = 5;

        private readonly TechnologyLevel[] universalLevels;

        private readonly TechnologyLevel[] antilegacyLevels;

        private bool useLegacy;

        private int desiredTechnologyLevelIndex = -1;

        public ExpandedTechnologyTree(XElement element, Parser<IReligion> religionParser)
        {
            var tierElements = (from tierElement in element.Elements() select tierElement).ToArray();
            var universalElements = (from tierElement in tierElements select tierElement.Element("universal")).ToArray();
            var antilegacyElements = (from tierElement in tierElements select tierElement.Element("antilegacy")).ToArray();
            this.universalLevels = (from levelElement in universalElements select new TechnologyLevel(this, levelElement)).ToArray();
            this.antilegacyLevels = (from levelElement in antilegacyElements select new TechnologyLevel(this, levelElement)).ToArray();
            for (var whichLevel = 0; whichLevel < TechnologyLevelsCount; ++whichLevel)
            {
                this.antilegacyLevels[whichLevel].Cumulate(this.universalLevels[whichLevel]);
            }

            for (var whichLevel = 1; whichLevel < TechnologyLevelsCount - 1; ++whichLevel)
            {
                this.universalLevels[whichLevel].Cumulate(this.universalLevels[whichLevel - 1]);
                this.antilegacyLevels[whichLevel].Cumulate(this.antilegacyLevels[whichLevel - 1]);
            }

            foreach (var level in this.universalLevels.Concat(this.antilegacyLevels))
            {
                foreach (var influence in level.Effect.Influences)
                {
                    influence.SetReligionParser(religionParser);
                }
            }
        }

        public event DesiredTechnologyLevelChangedHandler DesiredTechnologyChanged;

        public TechnologyLevel CurrentLevel => this.GetLevel(this.desiredTechnologyLevelIndex, this.useLegacy);

        public void ChangeDesiredTechnologyLevel(int tier, bool isLegacy)
        {
            this.desiredTechnologyLevelIndex = tier;
            this.useLegacy = isLegacy;
            this.OnDesiredTechnologyLevelChanged();
        }

        public bool IsLevelReasearched(TechnologyLevel level)
        {
            if (this.desiredTechnologyLevelIndex < 0)
            {
                throw new InvalidOperationException("Desired technology level is not set.");
            }

            for (var whichLevel = 0; whichLevel < TechnologyLevelsCount; ++whichLevel)
            {
                if (level == this.universalLevels[whichLevel])
                {
                    return whichLevel <= this.desiredTechnologyLevelIndex;
                }

                if (level == this.antilegacyLevels[whichLevel])
                {
                    if (this.useLegacy)
                    {
                        return false;
                    }

                    return whichLevel <= this.desiredTechnologyLevelIndex;
                }
            }

            throw new Exception("I have no idea what just happened.");
        }

        public TechnologyLevel GetLevel(int level, bool? isLegacy)
        {
            if (level < 0 || level >= TechnologyLevelsCount + 1)
            {
                throw new ArgumentOutOfRangeException(nameof(level), level, "Technology level out of range.");
            }

            if (!isLegacy.HasValue)
            {
                return this.universalLevels[level];
            }

            return this.antilegacyLevels[level];
        }

        private void OnDesiredTechnologyLevelChanged()
        {
            this.DesiredTechnologyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}