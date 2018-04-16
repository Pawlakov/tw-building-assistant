namespace Buildings
{
	public class BuildingHandle
	{
		// Interfejs wewnętrzny:
		//
		internal BuildingHandle(Building containedBuilding)
		{
			ContainedBuilding = containedBuilding;
			ResetUsefuliness();
		}
		//
		// Stan wewnętrzny / Interfejs publiczny:
		//
		/// <summary>
		/// Ile razy od ostatniego zresetowania użyteczności ten poziom budynku został użyty w działającej kombinacji.
		/// </summary>
		public int Usefuliness { get; private set; }
		public Building ContainedBuilding { get; }
		//
		// Interfejs publiczny:
		//
		/// <summary>
		/// Nagradza budynek za znalezienie się w działającej kombinacji.
		/// </summary>
		public void Reward()
		{
			++Usefuliness;
		}
		/// <summary>
		/// Zeruje użyteczność poziomu przygotowując go na kolejne jej zliczanie.
		/// </summary>
		public void ResetUsefuliness()
		{
			Usefuliness = 0;
		}
	}
}
