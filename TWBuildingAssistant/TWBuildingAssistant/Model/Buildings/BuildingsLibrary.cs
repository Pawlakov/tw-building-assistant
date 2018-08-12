namespace TWBuildingAssistant.Model.Buildings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using TWBuildingAssistant.Model.Religions;

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

        public BuildingLibrary(string fileName, ITechnologyLevelAssigner technologyLevelAssigner, Map.IResourceParser resourceParser, IReligionParser religionParser)
        {
            var sourceFile = XDocument.Load("Model\\Buildings\\" + fileName);
            var buildingCategories = (from XElement element in sourceFile.Root.Elements() select element).ToDictionary((XElement element) => (string)element.Attribute("n"));
            this.cityCenterBuilding = new BuildingBranch(buildingCategories["CenterCity"].Elements().First(), technologyLevelAssigner, religionParser);
            this.townCenterBuilding = new BuildingBranch(buildingCategories["CenterTown"].Elements().First(), technologyLevelAssigner, religionParser);
            this.coastBuilding = new BuildingBranch(buildingCategories["Coastal"].Elements().First(), technologyLevelAssigner, religionParser);
            var sharedBuildings = (from XElement element in buildingCategories["Shared"].Elements()
                                   select new BuildingBranch(element, technologyLevelAssigner, religionParser)).ToArray();
            var tempCityBuildings = from XElement element in buildingCategories["City"].Elements() select new BuildingBranch(element, technologyLevelAssigner, religionParser);
            var tempTownBuildings = from XElement element in buildingCategories["Town"].Elements() select new BuildingBranch(element, technologyLevelAssigner, religionParser);
            var tempResourceBuildings = from XElement element in buildingCategories["Resource"].Elements() select new BuildingBranch(element, technologyLevelAssigner, religionParser);
            this.cityBuildings = tempCityBuildings.Concat(sharedBuildings).ToArray();
            this.townBuildings = tempTownBuildings.Concat(sharedBuildings).ToArray();
            this.resourceBuildings = tempResourceBuildings.ToDictionary((branch) => resourceParser.Parse(branch.Name));
        }

        public IEnumerable<BuildingLevel> GetLevels(SlotType type, Resources.IResource resource)
        {
            var includeResource = resource != null
                                  && ((resource.BuildingType == Resources.SlotType.Coastal 
                                       && type == SlotType.Coast)
                                      || (resource.BuildingType == Resources.SlotType.Main 
                                          && (type == SlotType.CityCenter 
                                              || type == SlotType.TownCenter)) 
                                      || (resource.BuildingType == Resources.SlotType.Regular
                                          && (type == SlotType.City
                                              || type == SlotType.Town)));

            var includeRegular = !(includeResource && resource.IsMandatory);

            IEnumerable<BuildingLevel> result = new BuildingLevel[0];
            if (includeRegular)
            {
                switch (type)
                {
                    case SlotType.City:
                        result = (from BuildingBranch item in this.cityBuildings where item.IsAvailable select item).SelectMany((branch) => branch.Levels);
                        break;
                    case SlotType.Town:
                        result = (from BuildingBranch item in this.townBuildings where item.IsAvailable select item).SelectMany((branch) => branch.Levels);
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
            {
                result = result.Concat(this.resourceBuildings[resource].Levels);
            }

            result = from BuildingLevel item in result where item.IsAvailable select item;
            if (type == SlotType.City || type == SlotType.Town)
            {
                result = new BuildingLevel[] { null }.Concat(result);
            }

            return result;
        }
    }
}