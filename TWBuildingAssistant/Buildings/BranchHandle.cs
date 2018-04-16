using System;
using System.Linq;
using System.Collections.Generic;
namespace Buildings
{
	public class BranchHandle
	{
		// Interfejs wewnętrzny:
		//
		internal BranchHandle(Branch containedBranch)
		{
			ContainedBranch = containedBranch;
			HandledLevels = (from Building building in ContainedBranch.Levels where building.IsAvailable() select new BuildingHandle(building)).ToList();
		}
		//
		// Stan wewnętrzny:
		//
		private Branch ContainedBranch { get; }
		private List<BuildingHandle> HandledLevels { get; }
		//
		// Interfejs publiczny:
		//
		public int RelevantCount
		{
			get { return HandledLevels.Count; }
		}
		public void EvalueateLevels()
		{
			HandledLevels.RemoveAll((BuildingHandle level) => { return level.Usefuliness == 0; });
			foreach (BuildingHandle level in HandledLevels)
				level.ResetUsefuliness();
		}
		public BuildingHandle GetLevel()
		{
			int index;
			index = Utilities.XorShift.Next(0, HandledLevels.Count);
			return HandledLevels[index];
		}
		public BuildingHandle GetLevel(int desiredLevel)
		{
			while (!ContainsLevel(desiredLevel))
			{
				--desiredLevel;
				if (desiredLevel == 0)
					desiredLevel = 4;
			}
			int index;
			do
			{
				index = Utilities.XorShift.Next(0, HandledLevels.Count);
			} while (HandledLevels[index].ContainedBuilding.Level != desiredLevel);
			return HandledLevels[index];
		}
		public bool ContainsLevel(int desiredLevel)
		{
			foreach (BuildingHandle level in HandledLevels)
				if (level.ContainedBuilding.Level == desiredLevel)
					return true;
			return false;
		}
	}
}
