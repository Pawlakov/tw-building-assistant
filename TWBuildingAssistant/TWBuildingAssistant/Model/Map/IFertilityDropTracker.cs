namespace TWBuildingAssistant.Model.Map
{
    using System;

    public interface IFertilityDropTracker
    {
        event EventHandler<FertilityDropChangedEventArgs> FertilityDropChanged;

        int FertilityDrop { get; }
    }
}
