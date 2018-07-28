namespace TWBuildingAssistant.Model.Effects
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class WealthBonus
    {
        public int Value { get; }

        public WealthCategory Category { get; }

        public BonusType Type { get; }

        [JsonConstructor]
        public WealthBonus(int? value, WealthCategory? category, BonusType? type)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (!ValidateValues(value.Value, category.Value, type.Value, out var message))
                throw new ArgumentOutOfRangeException(message);
            Value = value.Value;
            Category = category.Value;
            Type = type.Value;
        }

        public void Execute(Dictionary<WealthCategory, WealthRecord> records)
        {
            if (records == null)
                throw new ArgumentNullException(nameof(records));
            if (!records.ContainsKey(Category))
                records.Add(Category, new WealthRecord());
            switch (Type)
            {
                case BonusType.Simple:
                    records[Category].AddToValue(Value);
                    break;
                case BonusType.Percentage:
                    records[Category].AddToPercents(Value);
                    break;
                case BonusType.FertilityDependent:
                    records[Category].AddToValuePerFertilityLevel(Value);
                    break;
            }
        }

        public override bool Equals(object obj)
        {
            return obj != null && (ReferenceEquals(this, obj) || Equals(obj as WealthBonus));
        }

        private bool Equals(WealthBonus other)
        {
            return other != null && (Value == other.Value && Category == other.Category && Type == other.Type);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Value;
                hashCode = (hashCode * 397) ^ (int)Category;
                hashCode = (hashCode * 397) ^ (int)Type;
                return hashCode;
            }
        }

        public static bool ValidateValues(int value, WealthCategory category, BonusType type, out string message)
        {
            switch (type)
            {
                case BonusType.Simple:
                    {
                        if (category == WealthCategory.All)
                        {
                            message = "Simple wealth bonus in 'All' category.";
                            return false;
                        }

                        if (category == WealthCategory.Maintenance)
                        {
                            if (value > 0)
                            {
                                message = "Positive bonus in 'Maintenance' category.";
                                return false;
                            }
                        }
                        else if (value < 0)
                        {
                            message = "Negative bonus outside of 'Maintenance' category.";
                            return false;
                        }

                        break;
                    }
                case BonusType.Percentage:
                    {
                        if (category == WealthCategory.Maintenance)
                        {
                            message = "Multiplier wealth bonus in 'Maintenance' category.";
                            return false;
                        }

                        if (value < 0)
                        {
                            message = "Negative multiplier bonus.";
                            return false;
                        }

                        break;
                    }
                case BonusType.FertilityDependent:
                    {
                        if (category != WealthCategory.Agriculture && category != WealthCategory.Husbandry)
                        {
                            message = "Fertility-dependent bonus outside of 'Agriculture' and 'Husbandry' categories.";
                            return false;
                        }

                        if (value < 0)
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

        public static WealthBonus Deserialize(string json)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));
            var settings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error };
            return JsonConvert.DeserializeObject<WealthBonus>(json, settings);
        }
    }
}