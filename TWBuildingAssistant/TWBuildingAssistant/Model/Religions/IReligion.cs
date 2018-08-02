namespace TWBuildingAssistant.Model.Religions
{
    using TWBuildingAssistant.Model.Effects;

    public interface IReligion
    {
        string Name { get; }

        IProvincionalEffect Effect { get; }

        bool Validate(out string message);

        bool IsState { get; }

        Map.IStateReligionTracker StateReligionTracker { set; }
    }
}
