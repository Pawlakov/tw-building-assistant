using System;
using System.Xml;
namespace Buildings
{
	class EvaluableBuilding : Building
	{
		internal EvaluableBuilding(XmlNode node) : base(node)
		{
			ResetUsefuliness();
		}
		/// <summary>
		/// Ile razy od ostatniego zresetowania użyteczności ten poziom budynku został użyty w działającej kombinacji.
		/// </summary>
		public int Usefuliness { get; private set; }
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
