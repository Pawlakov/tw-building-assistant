using System;
using System.Collections.Generic;
using System.Xml;
namespace TWAssistant
{
	namespace Attila
	{
		public class BuildingLibrary
		{
			BuildingBranch cityCivilBuilding;
			BuildingBranch townCivilBuilding;
			BuildingBranch coastBuilding;
			BuildingBranch spiceBuilding;
			//
			readonly BuildingBranch[] resourceBuildings;
			readonly List<BuildingBranch> cityBuildings;
			readonly List<BuildingBranch> townBuildings;
			//
			public BuildingLibrary(string filename)
			{
				cityBuildings = new List<BuildingBranch>();
				townBuildings = new List<BuildingBranch>();
				resourceBuildings = new BuildingBranch[Globals.ResourceTypesCount - 1];
				//
				XmlDocument sourceFile = new XmlDocument();
				sourceFile.Load(filename);
				XmlNodeList[] nodeList = new XmlNodeList[Globals.BuildingTypesCount];
				for (int whichType = 0; whichType < nodeList.Length; ++whichType)
					nodeList[whichType] = sourceFile.SelectNodes("//branch[@t=\"" + ((BuildingType)whichType).ToString() + "\"]");
				if (nodeList[(int)BuildingType.CENTERCITY].Count != 1 || nodeList[(int)BuildingType.CENTERCITY].Count != 1 || nodeList[(int)BuildingType.COAST].Count != 1 || nodeList[(int)BuildingType.SPICE].Count != 1)
					throw new Exception("Less or more than one civil, coast or spice buildings.");
				cityCivilBuilding = new BuildingBranch(nodeList[(int)BuildingType.CENTERCITY][0]);
				townCivilBuilding = new BuildingBranch(nodeList[(int)BuildingType.CENTERTOWN][0]);
				coastBuilding = new BuildingBranch(nodeList[(int)BuildingType.COAST][0]);
				spiceBuilding = new BuildingBranch(nodeList[(int)BuildingType.SPICE][0]);
				foreach (XmlNode node in nodeList[(int)BuildingType.RESOURCE])
				{
					Resource resource;
					resource = (Resource)Enum.Parse(typeof(Resource), node.Attributes.GetNamedItem("r").InnerText);
					if (resourceBuildings[(int)resource] != null)
						throw new Exception("Two building for one resource.");
					resourceBuildings[(int)resource] = new BuildingBranch(node);
				}
				foreach (XmlNode node in nodeList[(int)BuildingType.CITY])
					cityBuildings.Add(new BuildingBranch(node));
				foreach (XmlNode node in nodeList[(int)BuildingType.TOWN])
					townBuildings.Add(new BuildingBranch(node));
			}
			public BuildingLibrary(BuildingLibrary source)
			{
				cityCivilBuilding = new BuildingBranch(source.cityCivilBuilding);
				townCivilBuilding = new BuildingBranch(source.townCivilBuilding);
				coastBuilding = new BuildingBranch(source.coastBuilding);
				spiceBuilding = new BuildingBranch(source.spiceBuilding);
				//
				resourceBuildings = new BuildingBranch[source.resourceBuildings.Length];
				for (int whichBuilding = 0; whichBuilding < resourceBuildings.Length; ++whichBuilding)
					resourceBuildings[whichBuilding] = source.resourceBuildings[whichBuilding];
				//
				cityBuildings = new List<BuildingBranch>(source.cityBuildings.Count);
				for (int whichBuilding = 0; whichBuilding < source.cityBuildings.Count; ++whichBuilding)
					cityBuildings.Add(new BuildingBranch(source.cityBuildings[whichBuilding]));
				townBuildings = new List<BuildingBranch>(source.townBuildings.Count);
				for (int whichBuilding = 0; whichBuilding < source.townBuildings.Count; ++whichBuilding)
					townBuildings.Add(new BuildingBranch(source.townBuildings[whichBuilding]));
			}
			//
			public void ApplyLimitations()
			{
				for (int whichBranch = 0; whichBranch < cityBuildings.Count; ++whichBranch)
				{
					cityBuildings[whichBranch].ApplyTechnology();
					if (cityBuildings[whichBranch].isReligionExclusive && cityBuildings[whichBranch].religion != Globals.stateReligion)
					{
						cityBuildings.RemoveAt(whichBranch);
						--whichBranch;
					}
				}
				for (int whichBranch = 0; whichBranch < townBuildings.Count; ++whichBranch)
				{
					townBuildings[whichBranch].ApplyTechnology();
					if (townBuildings[whichBranch].isReligionExclusive && townBuildings[whichBranch].religion != Globals.stateReligion)
					{
						townBuildings.RemoveAt(whichBranch);
						--whichBranch;
					}
				}
				foreach (BuildingBranch building in resourceBuildings)
					if(building != null)
						building.ApplyTechnology();
				cityCivilBuilding.ApplyTechnology();
				townCivilBuilding.ApplyTechnology();
				coastBuilding.ApplyTechnology();
				spiceBuilding.ApplyTechnology();
			}
			public void ShowListOneType(BuildingType type)
			{
				List<BuildingBranch> list;
				switch (type)
				{
					case BuildingType.CITY:
						list = cityBuildings;
						break;
					case BuildingType.TOWN:
						list = townBuildings;
						break;
					default:
						list = null;
						break;
				}
				if (list != null)
					for (int whichBuilding = 0; whichBuilding < list.Count; ++whichBuilding)
					{
						Console.WriteLine("{0}. {1}", whichBuilding, list[whichBuilding].name);
					}
				else
					Console.WriteLine("No list for this type.");
			}
			public BuildingBranch GetExactBuilding(BuildingType type, int choice)
			{
				List<BuildingBranch> list;
				switch (type)
				{
					case BuildingType.CITY:
						list = cityBuildings;
						break;
					case BuildingType.TOWN:
						list = townBuildings;
						break;
					default:
						list = null;
						break;
				}
				if (list != null)
					return list[choice];
				Console.WriteLine("What do you think you're doing?");
				return null;
			}
			public int GetCountByType(BuildingType type)
			{
				List<BuildingBranch> list;
				switch (type)
				{
					case BuildingType.CITY:
						list = cityBuildings;
						break;
					case BuildingType.TOWN:
						list = townBuildings;
						break;
					case BuildingType.COAST:
						return coastBuilding.NonVoidCount;
					default:
						return 0;
				}
				int result = 0;
				foreach (BuildingBranch building in list)
				{
					result += building.NonVoidCount;
				}
				return result;
			}
			public void EvaluateBuildings()
			{
				List<BuildingBranch> list;
				coastBuilding.EvalueateLevels();
				list = cityBuildings;
				EvaluationHelper(list);
				list = townBuildings;
				EvaluationHelper(list);
			}
			void EvaluationHelper(List<BuildingBranch> list)
			{
				BuildingBranch building;
				for (int whichBuilding = 0; whichBuilding < list.Count; ++whichBuilding)
				{
					building = list[whichBuilding];
					building.EvalueateLevels();
					if (building.NonVoidCount == 0)
					{
						list.RemoveAt(whichBuilding);
						--whichBuilding;
					}
				}
			}
			public BuildingBranch GetBuilding(Resource resource)
			{
				return resourceBuildings[(int)resource];
			}
			public BuildingBranch GetBuilding(XorShift random, BuildingType type)
			{
				BuildingBranch result;
				switch (type)
				{
					case BuildingType.CENTERCITY:
						result = cityCivilBuilding;
						break;
					case BuildingType.CENTERTOWN:
						result = townCivilBuilding;
						break;
					case BuildingType.COAST:
						result = coastBuilding;
						break;
					case BuildingType.SPICE:
						result = spiceBuilding;
						break;
					case BuildingType.CITY:
						result = cityBuildings[(int)random.Next(0, (uint)cityBuildings.Count)];
						cityBuildings.Remove(result);
						break;
					case BuildingType.TOWN:
						result = townBuildings[(int)random.Next(0, (uint)townBuildings.Count)];
						townBuildings.Remove(result);
						break;
					default:
						throw new Exception("BuildingType should not be RESOURCE in this function.");
				}
				return result;
			}
			public void Remove(BuildingBranch building)
			{
				switch (building.type)
				{
					case BuildingType.CITY:
						cityBuildings.Remove(building);
						break;
					case BuildingType.TOWN:
						townBuildings.Remove(building);
						break;
					default:
						throw new Exception("What are you doing? You can't remove this building.");
				}
			}
		}
	}
}