namespace TWBuildingAssistant.Model.Effects
{
    public interface IInfluence
    {
        Religions.IReligion Religion { get; }
        
        int Value { get; }
        
        bool Validate(out string message);
    }
}