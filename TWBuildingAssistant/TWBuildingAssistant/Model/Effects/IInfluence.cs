namespace TWBuildingAssistant.Model.Effects
{
    using TWBuildingAssistant.Model.Religions;

    public interface IInfluence
    {
        IReligion GetReligion();
        
        int Value { get; }

        void SetReligionParser(IParser<IReligion> parser);

        bool Validate(out string message);
    }
}