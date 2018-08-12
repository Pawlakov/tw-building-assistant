namespace TWBuildingAssistant.Model.Religions
{
    using System;

    public class StateReligionChangedArgs : EventArgs
    {
        public IStateReligionTracker Tracker { get; }

        public IReligion NewStateReligion { get; }

        public StateReligionChangedArgs(IStateReligionTracker tracker, IReligion newStateReligion)
        {
            this.Tracker = tracker;
            this.NewStateReligion = newStateReligion;
        }
    }
}