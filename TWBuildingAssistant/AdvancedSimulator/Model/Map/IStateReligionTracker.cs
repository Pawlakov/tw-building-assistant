namespace TWBuildingAssistant.Model.Map
{
    using System;

    public interface IStateReligionTracker
    {
        event StateReligionChangedHandler StateReligionChanged;

        Religions.Religion StateReligion { get; }
    }

    public delegate void StateReligionChangedHandler(IStateReligionTracker sender, EventArgs e);
}
