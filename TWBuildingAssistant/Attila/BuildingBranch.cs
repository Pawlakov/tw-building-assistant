using System;
using System.Xml;
namespace TWAssistant
{
	namespace Attila //XD
	{
		public class BuildingBranch
		{
			readonly public string name;
			readonly public BuildingType type;
			//
			public BuildingLevel[] levels;
			//
			readonly public Resource resource;
			readonly public Religion? religion;
			readonly public bool isReligionExclusive;
			//
			public BuildingBranch(XmlNode branchNode)
			{
				name = branchNode.Attributes.GetNamedItem("n").InnerText;
				type = (BuildingType)Enum.Parse(typeof(BuildingType), branchNode.Attributes.GetNamedItem("t").InnerText);
				XmlNodeList levelNodeList = branchNode.ChildNodes;
				levels = new BuildingLevel[levelNodeList.Count];
				for (int whichLevel = 0; whichLevel < levels.Length; ++whichLevel)
					levels[whichLevel] = new BuildingLevel(levelNodeList.Item(whichLevel));
				//
				resource = Resource.NONE;
				religion = null;
				isReligionExclusive = false;
				XmlNode temporary = branchNode.Attributes.GetNamedItem("r");
				if (type == BuildingType.RESOURCE)
					resource = (Resource)Enum.Parse(typeof(Resource), temporary.InnerText);
				if (type == BuildingType.RESOURCE && resource == Resource.NONE)
					throw new Exception("Resource building with NONE resource(" + name + ").");
				//
				temporary = branchNode.Attributes.GetNamedItem("rel");
				if (temporary != null)
				{
					Religion temporaryReligion;
					temporaryReligion = (Religion)Enum.Parse(typeof(Religion), temporary.InnerText);
					religion = temporaryReligion;
					temporary = branchNode.Attributes.GetNamedItem("rex");
					isReligionExclusive = Convert.ToBoolean(temporary.InnerText);
				}
			}
			public BuildingBranch(BuildingBranch source)
			{
				name = source.name;
				type = source.type;
				//
				levels = new BuildingLevel[source.levels.Length];
				for (int whichLevel = 0; whichLevel < levels.Length; ++whichLevel)
					levels[whichLevel] = source.levels[whichLevel];
				//
				resource = source.resource;
				religion = source.religion;
				isReligionExclusive = source.isReligionExclusive;
			}
			//
			public int NonVoidCount
			{
				get { return levels.Length; }
			}
			public BuildingLevel this[int whichLevel]
			{
				get { return levels[whichLevel]; }
			}
			//
			public void EvalueateLevels()
			{
				int newSize = 0;
				foreach (BuildingLevel level in levels)
					if (level.Usefuliness > 0)
						++newSize;
				BuildingLevel[] newLevels = new BuildingLevel[newSize];
				int oldIndex = 0;
				int newIndex = 0;
				while (oldIndex < levels.Length)
				{
					if (levels[oldIndex].Usefuliness > 0)
					{
						newLevels[newIndex] = levels[oldIndex];
						++newIndex;
					}
					++oldIndex;
				}
				levels = newLevels;
				foreach (BuildingLevel level in levels)
					level.ResetUsefuliness();
			}
			public void ApplyTechnology()
			{
				int newSize = 0;
				foreach (BuildingLevel level in levels)
					if (level.IsAvailable)
						++newSize;
				BuildingLevel[] newLevels = new BuildingLevel[newSize];
				int oldIndex = 0;
				int newIndex = 0;
				while (oldIndex < levels.Length)
				{
					if (levels[oldIndex].IsAvailable)
					{
						newLevels[newIndex] = levels[oldIndex];
						++newIndex;
					}
					++oldIndex;
				}
				levels = newLevels;
			}
			public BuildingLevel GetLevel(XorShift random)
			{
				if (levels == null || levels.Length < 1)
					throw new Exception("Tried to get level from empty building.");
				int index;
				index = (int)random.Next(0, (uint)levels.Length);
				return levels[index];
			}
			public BuildingLevel GetLevel(XorShift random, int desiredLevel)
			{
				if (levels == null || levels.Length < 1)
					throw new Exception("Tried to get level from empty building.");
				while (!ContainsLevel(desiredLevel))
				{
					--desiredLevel;
					if (desiredLevel == 0)
						desiredLevel = 4;
				}
				int index;
				do
				{
					index = (int)random.Next(0, (uint)levels.Length);
				} while (levels[index].level != desiredLevel);
				return levels[index];
			}
			bool ContainsLevel(int desiredLevel)
			{
				foreach (BuildingLevel level in levels)
					if (level.level == desiredLevel)
						return true;
				return false;
			}
		}
	}
}