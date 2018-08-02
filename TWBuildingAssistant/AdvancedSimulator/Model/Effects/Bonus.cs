namespace TWBuildingAssistant.Model.Effects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents one of in-game income bonuses.
    /// </summary>
    public class Bonus : IBonus
    {
        /// <summary>
        /// Gets or sets the value of this <see cref="Bonus"/>.
        /// </summary>
        [Column]
        [Required]
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the category which this <see cref="Bonus"/> influence.
        /// </summary>
        [Column]
        [Required]
        public WealthCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the type of this <see cref="Bonus"/>' mechanism.
        /// </summary>
        [Column]
        [Required]
        public BonusType Type { get; set; }

        /// <summary>
        /// Executes this bonus on a given <see cref="Dictionary{WealthCategory,WealthRecord}"/> representing all wealth categories.
        /// </summary>
        /// <param name="records">
        /// The <see cref="Dictionary{WealthCategory,WealthRecord}"/> representing all wealth categories.
        /// </param>
        public void Execute(Dictionary<WealthCategory, WealthRecord> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            if (!records.ContainsKey(this.Category))
            {
                records.Add(this.Category, new WealthRecord());
            }

            Action<WealthRecord> action = null;

            switch (this.Type)
            {
                case BonusType.Simple:
                    action = record => { record.BaseValue += this.Value; };
                    break;
                case BonusType.Percentage:
                    action = record => { record.Percents += this.Value; };
                    break;
                case BonusType.FertilityDependent:
                    action = record => { record.ValuePerFertilityLevel += this.Value; };
                    break;
            }

            action?.Invoke(records[this.Category]);
        }

        /// <summary>
        /// Returns a value indicating whether the current combination of values is valid.
        /// </summary>
        /// <param name="message">
        /// Optional message containing details of vaildation's result.
        /// </param>
        /// <returns>
        /// The <see cref="bool" /> indicating result of validation.
        /// </returns>
        public bool Validate(out string message)
        {
            switch (this.Type)
            {
                case BonusType.Simple:
                    {
                        if (this.Category == WealthCategory.All)
                        {
                            message = "Simple wealth bonus in 'All' category.";
                            return false;
                        }

                        if (this.Category == WealthCategory.Maintenance)
                        {
                            if (this.Value > 0)
                            {
                                message = "Positive bonus in 'Maintenance' category.";
                                return false;
                            }
                        }
                        else if (this.Value < 0)
                        {
                            message = "Negative bonus outside of 'Maintenance' category.";
                            return false;
                        }

                        break;
                    }

                case BonusType.Percentage:
                    {
                        if (this.Category == WealthCategory.Maintenance)
                        {
                            message = "Multiplier wealth bonus in 'Maintenance' category.";
                            return false;
                        }

                        if (this.Value < 0)
                        {
                            message = "Negative multiplier bonus.";
                            return false;
                        }

                        break;
                    }

                case BonusType.FertilityDependent:
                    {
                        if (this.Category != WealthCategory.Agriculture && this.Category != WealthCategory.Husbandry)
                        {
                            message = "Fertility-dependent bonus outside of 'Agriculture' and 'Husbandry' categories.";
                            return false;
                        }

                        if (this.Value < 0)
                        {
                            message = "Negative fertility-dependent bonus.";
                            return false;
                        }

                        break;
                    }
            }

            message = "Values are correct.";
            return true;
        }

        /// <summary>
        /// Returns this <see cref="Bonus"/>' <see cref="string"/> representaion.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/> representation.
        /// </returns>
        public override string ToString()
        {
            return
            $"+{this.Value}{(this.Type == BonusType.Percentage ? "%" : string.Empty)} from {this.Category}{(this.Type == BonusType.FertilityDependent ? " for every fertility level" : string.Empty)}";
        }
    }
}