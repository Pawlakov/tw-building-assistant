using EnumsNET;
namespace GameWorld.WealthBonuses
{
	// Klasa bazowa dla różnych mechanizmów dochodów.
	public abstract class WealthBonus
	{
		protected WealthCategory Category { get; }
		protected WealthBonus(WealthCategory category)
		{
			Category = category;
		}
		//
		public static int WealthCategoriesCount { get; } = Enums.GetMemberCount<WealthCategory>();
		//
		public abstract void Execute(int[] values, int[] multipliers, int fertility);
	}
}
