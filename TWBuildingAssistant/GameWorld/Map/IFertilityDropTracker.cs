using System;
namespace GameWorld.Map
{
	public interface IFertilityDropTracker
	{
		event FertilityDropChangedEventHandler FertilityDropChanged;
		int FertilityDrop { get; }
	}
	public delegate void FertilityDropChangedEventHandler(IFertilityDropTracker sender, EventArgs e);
}
