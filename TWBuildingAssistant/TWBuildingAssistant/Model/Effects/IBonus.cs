namespace TWBuildingAssistant.Model.Effects
{
    public interface IBonus
    {
        int Value { get; }
        
        WealthCategory Category { get; }
        
        BonusType Type { get; }
        
        bool Validate(out string message);
    }
}