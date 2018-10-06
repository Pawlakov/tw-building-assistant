namespace TWBuildingAssistant.Model.Effects
{
    using System.Collections.Generic;

    public interface IBonus
    {
        int Value { get; }
        
        WealthCategory Category { get; }
        
        BonusType Type { get; }
        
        void Execute(Dictionary<WealthCategory, WealthRecord> records);
        
        bool Validate(out string message);
    }
}