namespace TWBuildingAssistant.Model.Technologies
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    using TWBuildingAssistant.Model.Religions;

    public class ExpandedTechnologyTree : ITechnologyTree
    {
        private const int TechnologyLevelsCount = 4;

        private readonly TechnologyLevel[] universalLevels;

        private readonly TechnologyLevel[] antilegacyLevels;

        private readonly TechnologyLevel[] legacyLevels;

        private readonly TechnologyLevel rootLevel;

        private bool useLegacy;

        private int desiredTechnologyLevelIndex = -1;

        public ExpandedTechnologyTree(XElement element, IParser<IReligion> religionParser)
        {
            this.rootLevel = new TechnologyLevel(this);
            var tierElements = (from tierElement in element.Elements() select tierElement).ToArray();
            var universalElements = (from tierElement in tierElements select tierElement.Element("universal")).ToArray();
            var antilegacyElements = (from tierElement in tierElements select tierElement.Element("antilegacy")).ToArray();
            this.universalLevels = (from levelElement in universalElements select new TechnologyLevel(this, levelElement)).ToArray();
            this.antilegacyLevels = (from levelElement in antilegacyElements select new TechnologyLevel(this, levelElement)).ToArray();
            this.legacyLevels = (from levelElement in antilegacyElements select new TechnologyLevel(this)).ToArray();
            for (var whichLevel = 0; whichLevel < TechnologyLevelsCount - 1; ++whichLevel)
            {
                this.antilegacyLevels[whichLevel].Cumulate(this.universalLevels[whichLevel]);
                this.legacyLevels[whichLevel].Cumulate(this.universalLevels[whichLevel]);
            }

            for (var whichLevel = 1; whichLevel < TechnologyLevelsCount - 1; ++whichLevel)
            {
                this.universalLevels[whichLevel].Cumulate(this.universalLevels[whichLevel - 1]);
                this.antilegacyLevels[whichLevel].Cumulate(this.antilegacyLevels[whichLevel - 1]);
                this.legacyLevels[whichLevel].Cumulate(this.legacyLevels[whichLevel - 1]);
            }

            foreach (var level in this.universalLevels.Concat(this.legacyLevels).Concat(this.antilegacyLevels).Append(this.rootLevel))
            {
                foreach (var influence in level.Effect.Influences)
                {
                    influence.ReligionParser = religionParser;
                }
            }
        }

        public event DesiredTechnologyLevelChangedHandler DesiredTechnologyChanged;

        public TechnologyLevel CurrentLevel => this.GetLevel(this.desiredTechnologyLevelIndex, this.useLegacy);

        public void ChangeDesiredTechnologyLevel(int tier, bool useLegacy)
        {
            this.desiredTechnologyLevelIndex = tier;
            this.useLegacy = useLegacy;
            this.OnDesiredTechnologyLevelChanged();
        }

        public bool IsLevelReasearched(TechnologyLevel level)
        {
            if (this.desiredTechnologyLevelIndex < 0)
            {
                throw new InvalidOperationException("Desired technology level is not set.");
            }

            if (level == this.rootLevel)
            {
                return true;
            }

            for (var whichLevel = 0; whichLevel < TechnologyLevelsCount; ++whichLevel)
            {
                if (level == this.universalLevels[whichLevel])
                {
                    return whichLevel + 1 <= this.desiredTechnologyLevelIndex;
                }

                if (level == this.antilegacyLevels[whichLevel])
                {
                    if (this.useLegacy)
                    {
                        return false;
                    }
                    else
                    {
                        return whichLevel + 1 <= this.desiredTechnologyLevelIndex;
                    }
                }

                if (level == this.legacyLevels[whichLevel])
                {
                    if (this.useLegacy)
                    {
                        return true;
                    }
                    else
                    {
                        return whichLevel + 1 > this.desiredTechnologyLevelIndex;
                    }
                }
            }

            return false;
        }

        public TechnologyLevel GetLevel(int level, bool? isLegacy)
        {
            if (level < 0 || level >= TechnologyLevelsCount + 1)
            {
                throw new ArgumentOutOfRangeException(nameof(level), level, "Technology level out of range.");
            }

            if (level == 0)
            {
                return this.rootLevel;
            }

            if (!isLegacy.HasValue)
            {
                return this.universalLevels[level - 1];
            }

            if (isLegacy.Value)
            {
                return this.legacyLevels[level - 1];
            }

            return this.antilegacyLevels[level - 1];
        }

        private void OnDesiredTechnologyLevelChanged()
        {
            this.DesiredTechnologyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}