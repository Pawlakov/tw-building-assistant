using System;
namespace GameWorld.Map
{
	public interface IStateReligionTracker
	{
		event StateReligionChangedHandler StateReligionChanged;
		Religions.Religion StateReligion { get; }
	}
	public delegate void StateReligionChangedHandler(IStateReligionTracker sender, EventArgs e);
}
