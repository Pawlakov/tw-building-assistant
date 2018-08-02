namespace TWBuildingAssistant.Model.Map
{
    using System;

    public interface IFertilityDropTracker
    {
        event FertilityDropChangedEventHandler FertilityDropChanged;

        int FertilityDrop { get; }
    }

    public delegate void FertilityDropChangedEventHandler(IFertilityDropTracker sender, EventArgs e);
}
