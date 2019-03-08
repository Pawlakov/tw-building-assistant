namespace TWBuildingAssistant.Model.Buildings
{
    using System.Linq;

    using TWBuildingAssistant.Model.Religions;
    using TWBuildingAssistant.Model.Weather;

    using Unity;

    public class BuildingManager : Parser<IBuilding>
    {
        public BuildingManager(IUnityContainer resolver)
        {
            this.Content = resolver.Resolve<ISource>().Buildings.ToArray();

            foreach (var building in this.Content)
            {
                building.SetReligionParser(resolver.Resolve<Parser<IReligion>>());
                // building.SetTechnologyLevelAssigner(resolver.Resolve<ITechnologyLevelAssigner>());
                // To nie ma sensu w ten sposób, trzeba już przerabiać głęboko.
                foreach (var influence in building.Effect.Influences)
                {
                    influence.SetReligionParser(resolver.Resolve<Parser<IReligion>>());
                }

                if (!building.Validate(out string message))
                {
                    throw new BuildingsException($"One of buildings is not valid ({message}).");
                }
            }
        }
    }
}
