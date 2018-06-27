using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
namespace GameWorld.Buildings
{
	// Miejsca na budynku mogą przyjmować budynki konkretnego typu. To są kategorie.
	public enum SlotType
	{
		CityCenter,
		TownCenter,
		Coast,
		City,
		Town
	}
	// Obiekt tej klasy jest zbiorem budynków jednej nacji.
	public class BuildingLibrary
	{
		private readonly BuildingBranch _cityCenterBuilding;
		private readonly BuildingBranch _townCenterBuilding;
		private readonly BuildingBranch[] _cityBuildings;
		private readonly BuildingBranch[] _townBuildings;
		private readonly BuildingBranch _coastBuilding;
		private readonly Dictionary<Resources.Resource, BuildingBranch> _resourceBuildings;
		//
		public BuildingLibrary(string fileName, ITechnologyLevelAssigner technologyLevelAssigner, Map.IResourceParser resourceParser, Map.IReligionParser religionParser)
		{
			XDocument sourceFile = XDocument.Load("Buildings\\" + fileName);
			Dictionary<string, XElement> buildingCategories = (from XElement element in sourceFile.Root.Elements() select element).ToDictionary((XElement element) => (string)element.Attribute("n"));
			_cityCenterBuilding = new BuildingBranch(buildingCategories["CenterCity"].Elements().First(), technologyLevelAssigner, religionParser);
			_townCenterBuilding = new BuildingBranch(buildingCategories["CenterTown"].Elements().First(), technologyLevelAssigner, religionParser);
			_coastBuilding = new BuildingBranch(buildingCategories["Coast"].Elements().First(), technologyLevelAssigner, religionParser);
			var sharedBuildings = from XElement element in buildingCategories["Shared"].Elements() select new BuildingBranch(element, technologyLevelAssigner, religionParser);
			var cityBuildings = from XElement element in buildingCategories["City"].Elements() select new BuildingBranch(element, technologyLevelAssigner, religionParser);
			var townBuildings = from XElement element in buildingCategories["Town"].Elements() select new BuildingBranch(element, technologyLevelAssigner, religionParser);
			var resourceBuildings = from XElement element in buildingCategories["Resource"].Elements() select new BuildingBranch(element, technologyLevelAssigner, religionParser);
			_cityBuildings = cityBuildings.Concat(sharedBuildings).ToArray();
			_townBuildings = townBuildings.Concat(sharedBuildings).ToArray();
			_resourceBuildings = resourceBuildings.ToDictionary((BuildingBranch branch) => resourceParser.Parse(branch.Name));
		}
		// Zwraca zbiór budynków dostępnych dla danego typu slotu i możliwego zasobu.
		public IEnumerable<BuildingLevel> GetLevels(SlotType type, Resources.Resource resource)
		{
			// Czy do zbioru dołączyć budynek specjalny zasobu?
			bool includeResource = resource != null && 
				(
				(resource.BuildingType == Resources.Resource.ReplacedBuildingType.Coast && type == SlotType.Coast) ||
				(resource.BuildingType == Resources.Resource.ReplacedBuildingType.Main && (type == SlotType.CityCenter || type == SlotType.TownCenter)) ||
				(resource.BuildingType == Resources.Resource.ReplacedBuildingType.General && (type == SlotType.City || type == SlotType.Town))
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
						result = (from BuildingBranch item in _cityBuildings where item.IsAvailable select item).SelectMany((BuildingBranch branch) => branch.Levels);
						break;
					case SlotType.Town:
						result = (from BuildingBranch item in _townBuildings where item.IsAvailable select item).SelectMany((BuildingBranch branch) => branch.Levels);
						break;
					case SlotType.CityCenter:
						result = _cityCenterBuilding.Levels;
						break;
					case SlotType.TownCenter:
						result = _townCenterBuilding.Levels;
						break;
					case SlotType.Coast:
						result = _coastBuilding.Levels;
						break;
				}
			}
			if (includeResource)
				result = result.Concat(_resourceBuildings[resource].Levels);
			// Usuń te poziomy które są niedostępne.
			result = from BuildingLevel item in result where item.IsAvailable select item;
			// Na polach tego typu można nie mieć żadnego budynku.
			if (type == SlotType.City || type == SlotType.Town)
				result = new BuildingLevel[1] { null }.Concat(result);
			return result;
		}
	}
}