namespace TWBuildingAssistant.Model.Resources
{
    /// <summary>
    /// Respresents one of possible categories of buidling slot.
    /// </summary>
    public enum SlotType
    {
        /// <summary>
        /// Slot provided for a settlement's main building.
        /// </summary>
        Main,

        /// <summary>
        /// Slot provided for a regular building.
        /// </summary>
        Regular,

        /// <summary>
        /// Slot provided for a coastal settlement's port building.
        /// </summary>
        Coastal
    }
}