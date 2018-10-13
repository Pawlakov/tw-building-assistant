namespace TWBuildingAssistant.Model.Climate
{
    using System.Collections.Generic;

    public interface IClimateSource
    {
        IEnumerable<IClimate> Climates { get; }
    }
}