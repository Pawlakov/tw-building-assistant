namespace TWBuildingAssistant.Model.Religions
{
    using System;

    public interface IStateReligionTracker
    {
        event EventHandler<StateReligionChangedArgs> StateReligionChanged;

        IReligion StateReligion { get; }
    }
}