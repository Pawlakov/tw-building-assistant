namespace TWBuildingAssistant.Model.Effects
{
    using TWBuildingAssistant.Model.Religions;

    public interface IInfluence
    {
        IReligion GetReligion();
        
        int Value { get; }

        void SetReligionParser(Parser<IReligion> parser);

        bool Validate(out string message);
    }
}