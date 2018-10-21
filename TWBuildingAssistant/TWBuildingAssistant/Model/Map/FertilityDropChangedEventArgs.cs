namespace TWBuildingAssistant.Model.Map
{
    using System;

    public class FertilityDropChangedEventArgs : EventArgs
    {
        public FertilityDropChangedEventArgs(IFertilityDropTracker tracker, int newFertilityDrop)
        {
            this.Tracker = tracker;
            this.NewFertilityDrop = newFertilityDrop;
        }

        public IFertilityDropTracker Tracker { get; }

        public int NewFertilityDrop { get; }
    }
}