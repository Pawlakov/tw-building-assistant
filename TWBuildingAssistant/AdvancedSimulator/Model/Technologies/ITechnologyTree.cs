namespace TWBuildingAssistant.Model.Technologies
{
    using System;

    public interface ITechnologyTree : Buildings.ITechnologyLevelAssigner
    {
        event DesiredTechnologyLevelChangedHandler DesiredTechnologyChanged;

        void ChangeDesiredTechnologyLevel(int whichLevel, bool useLegacyTechnolgies);

        bool IsLevelReasearched(TechnologyLevel level);

        TechnologyLevel CurrentLevel { get; }
    }

    public delegate void DesiredTechnologyLevelChangedHandler(ITechnologyTree sender, EventArgs e);
}
