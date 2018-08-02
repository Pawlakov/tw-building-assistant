namespace TWBuildingAssistant.Model.Effects
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents one of in-game income bonuses.
    /// </summary>
    public interface IBonus
    {
        /// <summary>
        /// Gets the value of this <see cref="IBonus"/>.
        /// </summary>
        int Value { get; }

        /// <summary>
        /// Gets the category which this <see cref="IBonus"/> influence.
        /// </summary>
        WealthCategory Category { get; }

        /// <summary>
        /// Gets the type of this <see cref="IBonus"/>' mechanism.
        /// </summary>
        BonusType Type { get; }

        /// <summary>
        /// Executes this bonus on a given <see cref="Dictionary{WealthCategory,WealthRecord}"/> representing all wealth categories.
        /// </summary>
        /// <param name="records">
        /// The <see cref="Dictionary{WealthCategory,WealthRecord}"/> representing all wealth categories.
        /// </param>
        void Execute(Dictionary<WealthCategory, WealthRecord> records);

        /// <summary>
        /// Returns a value indicating whether the current combination of values is valid.
        /// </summary>
        /// <param name="message">
        /// Optional message containing details of vaildation's result.
        /// </param>
        /// <returns>
        /// The <see cref="bool" /> indicating result of validation.
        /// </returns>
        bool Validate(out string message);
    }
}