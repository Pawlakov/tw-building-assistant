namespace TWBuildingAssistant.Model.Buildings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public enum SlotType
    {
        CityCenter,

        TownCenter,

        Coast,

        City,

        Town
    }

    public class BuildingLibrary
    {
        private readonly BuildingBranch cityCenterBuilding;

        private readonly BuildingBranch townCenterBuilding;

        private readonly BuildingBranch[] cityBuildings;

        private readonly BuildingBranch[] townBuildings;

        private readonly BuildingBranch coastBuilding;

        private readonly Dictionary<Resources.IResource, BuildingBranch> resourceBuildings;

        public BuildingLibrary(string fileName, ITechnologyLevelAssigner technologyLevelAssigner, Map.IResourceParser resourceParser, Map.IReligionParser religionParser)
        {
            XDocument sourceFile = XDocument.Load("Buildings\\" + fileName);
            Dictionary<string, XElement> buildingCategories = (from XElement element in sourceFile.Root.Elements() select element).ToDictionary((XElement element) => (string)element.Attribute("n"));
            this.cityCenterBuilding = new BuildingBranch(buildingCategories["CenterCity"].Elements().First(), technologyLevelAssigner, religionParser);
            this.townCenterBuilding = new BuildingBranch(buildingCategories["CenterTown"].Elements().First(), technologyLevelAssigner, religionParser);
            this.coastBuilding = new BuildingBranch(buildingCategories["Coastal"].Elements().First(), technologyLevelAssigner, religionParser);
            var sharedBuildings = from XElement element in buildingCategories["Shared"].Elements() select new BuildingBranch(element, technologyLevelAssigner, religionParser);
            var cityBuildings = from XElement element in buildingCategories["City"].Elements() select new BuildingBranch(element, technologyLevelAssigner, religionParser);
            var townBuildings = from XElement element in buildingCategories["Town"].Elements() select new BuildingBranch(element, technologyLevelAssigner, religionParser);
            var resourceBuildings = from XElement element in buildingCategories["Resource"].Elements() select new BuildingBranch(element, technologyLevelAssigner, religionParser);
            this.cityBuildings = cityBuildings.Concat(sharedBuildings).ToArray();
            this.townBuildings = townBuildings.Concat(sharedBuildings).ToArray();
            this.resourceBuildings = resourceBuildings.ToDictionary((BuildingBranch branch) => resourceParser.Parse(branch.Name));
        }

        public IEnumerable<BuildingLevel> GetLevels(SlotType type, Resources.IResource resource)
        {
            // Czy do zbioru dołączyć budynek specjalny zasobu?
            bool includeResource = resource != null &&
                (
                (resource.BuildingType == Resources.SlotType.Coastal && type == SlotType.Coast) ||
                (resource.BuildingType == Resources.SlotType.Main && (type == SlotType.CityCenter || type == SlotType.TownCenter)) ||
                (resource.BuildingType == Resources.SlotType.Regular && (type == SlotType.City || type == SlotType.Town))
                );
            // Jeżeli zasób MUSI być wydobywany to nie można postawić innego budynku.
            bool includeRegular = !(includeResource && resource.IsMandatory);
            // Składanie wynikowego zbioru.
            IEnumerable<BuildingLevel> result = new BuildingLevel[0];
            if (includeRegular)
            {
                switch (type)
                {
                    case SlotType.City:
                        result = (from BuildingBranch item in this.cityBuildings where item.IsAvailable select item).SelectMany((BuildingBranch branch) => branch.Levels);
                        break;
                    case SlotType.Town:
                        result = (from BuildingBranch item in this.townBuildings where item.IsAvailable select item).SelectMany((BuildingBranch branch) => branch.Levels);
                        break;
                    case SlotType.CityCenter:
                        result = this.cityCenterBuilding.Levels;
                        break;
                    case SlotType.TownCenter:
                        result = this.townCenterBuilding.Levels;
                        break;
                    case SlotType.Coast:
                        result = this.coastBuilding.Levels;
                        break;
                }
            }
            if (includeResource)
                result = result.Concat(this.resourceBuildings[resource].Levels);
            // Usuń te poziomy które są niedostępne.
            result = from BuildingLevel item in result where item.IsAvailable select item;
            // Na polach tego typu można nie mieć żadnego budynku.
            if (type == SlotType.City || type == SlotType.Town)
                result = new BuildingLevel[1] { null }.Concat(result);
            return result;
        }
    }
}