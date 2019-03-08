namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;

    using TWBuildingAssistant.Model.Buildings;
    using TWBuildingAssistant.Model.Climate;
    using TWBuildingAssistant.Model.Map;
    using TWBuildingAssistant.Model.Religions;
    using TWBuildingAssistant.Model.Resources;
    using TWBuildingAssistant.Model.Weather;

    public interface ISource
    {
        IEnumerable<IReligion> Religions { get; }

        IEnumerable<IResource> Resources { get; }

        IEnumerable<IClimate> Climates { get; }

        IEnumerable<IWeather> Weathers { get; }

        IEnumerable<IProvince> Provinces { get; }

        IEnumerable<IBuilding> Buildings { get; }

        IEnumerable<IBranch> Branches { get; }
    }
}