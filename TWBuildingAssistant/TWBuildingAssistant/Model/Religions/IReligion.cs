namespace TWBuildingAssistant.Model.Religions
{
    using TWBuildingAssistant.Model.Effects;

    public interface IReligion : IParsable
    {
        IProvincialEffect Effect { get; }

        bool IsState { get; }
        
        IStateReligionTracker StateReligionTracker { set; }
        
        bool Validate(out string message);
    }
}
