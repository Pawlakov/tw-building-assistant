using System;
namespace GameWorld.Technologies
{
	public interface ITechnologyTree : Buildings.ITechnologyLevelAssigner
	{
		event DesiredTechnologyLevelChangedHandler DesiredTechnologyChanged;
		void ChangeDesiredTechnologyLevel(int whichLevel, bool useLegacyTechnolgies);
		bool IsLevelReasearched(TechnologyLevel level);
		TechnologyLevel CurrentLevel { get; }
	}
	public delegate void DesiredTechnologyLevelChangedHandler(ITechnologyTree sender, EventArgs e);
}
