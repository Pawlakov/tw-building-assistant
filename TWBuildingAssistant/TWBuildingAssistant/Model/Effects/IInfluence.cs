namespace TWBuildingAssistant.Model.Effects
{
    using TWBuildingAssistant.Model.Religions;

    public interface IInfluence
    {
        IReligion GetReligion();
        
        int Value { get; }

        IParser<IReligion> ReligionParser { set; }

        bool Validate(out string message);
    }
}