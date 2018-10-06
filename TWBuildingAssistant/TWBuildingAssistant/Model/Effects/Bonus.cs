namespace TWBuildingAssistant.Model.Effects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Bonus : IBonus
    {
        public int Value { get; set; }

        public WealthCategory Category { get; set; }

        public BonusType Type { get; set; }

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

            records[this.Category][this.Type] += this.Value;
        }
        
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
        
        public override string ToString()
        {
            return
            $"+{this.Value}{(this.Type == BonusType.Percentage ? "%" : string.Empty)} from {this.Category}{(this.Type == BonusType.FertilityDependent ? " for every fertility level" : string.Empty)}";
        }
    }
}