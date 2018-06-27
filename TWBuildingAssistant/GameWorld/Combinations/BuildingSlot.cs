using System;
namespace GameWorld.Combinations
{
	// Miejsce w którym może być postawiony budynek.
	public class BuildingSlot
	{
		public BuildingSlot(Buildings.SlotType type)
		{
			Type = type;
		}
		public override string ToString()
		{
			return string.Format
				(
					"Type: {0} Building: {1}",
					Type, 
					Level != null ? Level.ToString() : "???"
				);
		}
		//
		// Typ slotu, czyli co może się w nim znaleźć.
		public Buildings.SlotType Type { get; }
		// Budynek znajdujący się w tym slocie (może być żaden).
		public Buildings.BuildingLevel Level { get; set; }
	}
}
