namespace TWBuildingAssistant.Model.Effects
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    using EnumsNET;

    public static class BonusFactory
    {
        public static IBonus MakeBonus(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (!ValidateElement(element, out var message))
                throw new FormatException(message);
            BonusType type;
            if (element.Name == "multiplier")
                type = BonusType.Percentage;
            else if (element.Name == "fertility_dependent")
                type = BonusType.FertilityDependent;
            else
                type = BonusType.Simple;
            return new Bonus() { Value = (int)element, Category = Enums.Parse<WealthCategory>((string)element.Attribute("c")), Type = type };
        }

        public static bool ValidateElement(XElement element, out string message)
        {
            if (element.Name != "multiplier" && element.Name != "bonus" && element.Name != "fertility_dependent")
            {
                message = "XML element doesn't describe any wealth bonus.";
                return false;
            }

            if (element.Attribute("c") == null)
            {
                message = "XML element lacks 'c' attribute.";
                return false;
            }

            if (!Enums.TryParse<WealthCategory>((string)element.Attribute("c"), out _))
            {
                message = "XML element's 'c' attribute is not a WealthCategory value.";
                return false;
            }

            if (element.Attributes().Count() != 1)
            {
                message = "XML element has too many or too few attributes.";
                return false;
            }

            if (!int.TryParse((string)element, out var __))
            {
                message = "XML element's content is not an integer value.";
                return false;
            }

            // UWAGA: Tu zostało pominięta walidacja wewnątrz bonusu.

            message = "XML element is a valid representation of a wealth bonus.";
            return true;
        }
    }
}
