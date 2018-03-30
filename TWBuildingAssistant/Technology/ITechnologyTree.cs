using System;
namespace Technologies
{
	public interface ITechnologyTree
	{
		event DesiredTechnologyLevelChangedHandler DesiredTechnologyChanged;
		event DesiredTechnologyLevelChangingHandler DesiredTechnologyChanging;
		void ChangeDesiredTechnologyLevel();
		bool IsLevelReasearched(TechnologyLevel level);
		TechnologyLevel Parse(int level, bool? isLegacy);
	}
	public delegate void DesiredTechnologyLevelChangedHandler(ITechnologyTree sender, EventArgs e);
	public delegate void DesiredTechnologyLevelChangingHandler(ITechnologyTree sender, EventArgs e);
}