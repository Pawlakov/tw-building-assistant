namespace TWBuildingAssistant.Model.Map
{
    using System.Collections.Generic;

    using TWBuildingAssistant.Model.Climate;
    using TWBuildingAssistant.Model.Effects;

    public interface IProvince : IParsable
    {
        int Fertility { get; }
        
        int ClimateId { get; }
        
        IEnumerable<IRegion> Regions { get; }
        
        IProvincialEffect Effect { get; }

        int GetCurrentFertility();

        IClimate GetClimate();

        void SetClimateParser(Parser<IClimate> parser);

        void SetFertilityDropTracker(IFertilityDropTracker tracker);

        bool Validate(out string message);
    }
}