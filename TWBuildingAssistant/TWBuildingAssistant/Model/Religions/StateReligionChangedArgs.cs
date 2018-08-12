namespace TWBuildingAssistant.Model.Religions
{
    using System;

    public class StateReligionChangedArgs : EventArgs
    {
        public IStateReligionTracker Tracker { get; }

        public StateReligionChangedArgs(IStateReligionTracker tracker)
        {
            this.Tracker = tracker;
        }
    }
}