namespace TWBuildingAssistant.Model.Buildings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using TWBuildingAssistant.Model.Religions;
    using TWBuildingAssistant.Model.Resources;

    public class BuildingLibrary
    {
        private readonly Branch cityCenterBuilding;

        private readonly Branch townCenterBuilding;

        private readonly Branch[] cityBuildings;

        private readonly Branch[] townBuildings;

        private readonly Branch coastBuilding;

        private readonly Dictionary<Resources.IResource, Branch> resourceBuildings;

        public BuildingLibrary(string fileName, ITechnologyLevelAssigner technologyLevelAssigner, Parser<IResource> resourceParser, Parser<IReligion> religionParser)
        {
            var sourceFile = XDocument.Load("Model\\Buildings\\" + fileName);
            var buildingCategories = (from XElement element in sourceFile.Root.Elements() select element).ToDictionary((XElement element) => (string)element.Attribute("n"));
            this.cityCenterBuilding = new Branch(buildingCategories["CenterCity"].Elements().First(), technologyLevelAssigner, religionParser);
            this.townCenterBuilding = new Branch(buildingCategories["CenterTown"].Elements().First(), technologyLevelAssigner, religionParser);
            this.coastBuilding = new Branch(buildingCategories["Coastal"].Elements().First(), technologyLevelAssigner, religionParser);
            var sharedBuildings = (from XElement element in buildingCategories["Shared"].Elements()
                                   select new Branch(element, technologyLevelAssigner, religionParser)).ToArray();
            var tempCityBuildings = from XElement element in buildingCategories["City"].Elements() select new Branch(element, technologyLevelAssigner, religionParser);
            var tempTownBuildings = from XElement element in buildingCategories["Town"].Elements() select new Branch(element, technologyLevelAssigner, religionParser);
            var tempResourceBuildings = from XElement element in buildingCategories["Resource"].Elements() select new Branch(element, technologyLevelAssigner, religionParser);
            this.cityBuildings = tempCityBuildings.Concat(sharedBuildings).ToArray();
            this.townBuildings = tempTownBuildings.Concat(sharedBuildings).ToArray();
            this.resourceBuildings = tempResourceBuildings.ToDictionary((branch) => resourceParser.Parse(branch.Name));

            //var all = tempResourceBuildings.SelectMany(x => x.Levels)
            //    .Concat(this.coastBuilding.Levels)
            //    .Concat(sharedBuildings.SelectMany(x => x.Levels))
            //    .Concat(tempTownBuildings.SelectMany(x => x.Levels))
            //    .Concat(tempCityBuildings.SelectMany(x => x.Levels))
            //    .Concat(this.townCenterBuilding.Levels)
            //    .Concat(this.cityCenterBuilding.Levels)
            //    .OrderBy(x => x.Name)
            //    .ToList();
            //for (var id = 0; id < all.Count; ++id)
            //{
            //    all[id].Id = id + 1;
            //}

            //File.WriteAllText(@"Json\temp_buildings.json", JsonConvert.SerializeObject(all, Formatting.Indented));
        }

        public IEnumerable<Building> GetLevels(SlotType type, Resources.IResource resource)
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

            var includeRegular = !(includeResource && resource.Obligatory);

            IEnumerable<Building> result = new Building[0];
            if (includeRegular)
            {
                switch (type)
                {
                    case SlotType.City:
                        result = (from Branch item in this.cityBuildings where item.IsAvailable select item).SelectMany((branch) => branch.Levels);
                        break;
                    case SlotType.Town:
                        result = (from Branch item in this.townBuildings where item.IsAvailable select item).SelectMany((branch) => branch.Levels);
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

            result = from Building item in result where item.CheckIsAvailable() select item;
            if (type == SlotType.City || type == SlotType.Town)
            {
                result = new Building[] { null }.Concat(result);
            }

            return result;
        }
    }
}