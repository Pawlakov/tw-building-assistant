namespace TWBuildingAssistant.Model.Effects
{
    /// <summary>
    /// Represents the type of bonus (it's mechanism of action).
    /// </summary>
    public enum BonusType
    {
        /// <summary>
        /// The simple type of bonus (additive).
        /// </summary>
        Simple,

        /// <summary>
        /// The percentage-based multiplicative bonus.
        /// </summary>
        Percentage,

        /// <summary>
        /// The fertility dependent additive bonus (the value is first multiplied by fertility level).
        /// </summary>
        FertilityDependent
    }
}
