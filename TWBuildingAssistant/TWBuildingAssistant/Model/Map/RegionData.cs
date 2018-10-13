namespace TWBuildingAssistant.Model.Map
{
    using System;
    using System.Xml.Linq;

    using TWBuildingAssistant.Model.Resources;

    public class RegionData
    {
        private const int CityDefaultSlotsCount = 6;

        private const int TownDefaultSlotsCount = 4;

        private readonly int slotsCountOffset;

        public RegionData(XElement element, bool isCity, IParser<IResource> resourceParser)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (resourceParser == null)
            {
                throw new ArgumentNullException(nameof(resourceParser));
            }

            if (!ValidateElement(element, resourceParser, out string message))
            {
                throw new FormatException(message);
            }

            this.IsCity = isCity;
            this.Resource = null;
            this.slotsCountOffset = 0;
            this.Name = (string)element.Attribute("n");
            if (element.Attribute("r") != null)
            {
                this.Resource = resourceParser.Parse((string)element.Attribute("r"));
            }

            this.IsCoastal = (bool)element.Attribute("ic");
            if (element.Attribute("o") != null)
            {
                this.slotsCountOffset = (int)element.Attribute("o");
            }
        }

        public string Name { get; }

        public Resources.IResource Resource { get; }

        public bool IsCoastal { get; }

        public bool IsCity { get; }

        public int SlotsCount
        {
            get
            {
                if (this.IsCity)
                {
                    return CityDefaultSlotsCount + this.slotsCountOffset;
                }

                return TownDefaultSlotsCount + this.slotsCountOffset;
            }
        }

        public static bool ValidateElement(XElement element, IParser<IResource> resourceParser, out string message)
        {
            if (element.Attribute("n") == null)
            {
                message = "XML element lacks 'n' attribute.";
                return false;
            }

            if (element.Attribute("ic") == null)
            {
                message = "XML element lacks 'ic' attribute (name: " + (string)element.Attribute("n") + ").";
                return false;
            }

            if (element.Attribute("r") != null)
            {
                if (resourceParser.Parse((string)element.Attribute("r")) == null)
                {
                    message = "XML element's 'r' attribute is not a resource name (name: " + (string)element.Attribute("n") + ").";
                    return false;
                }
            }

            if (element.Attribute("o") != null)
            {
                if (!int.TryParse((string)element.Attribute("o"), out _))
                {
                    message = "XML element's 'o' attribute is not an integer value (name: " + (string)element.Attribute("n") + ").";
                    return false;
                }
            }

            message = "XML element is a valid representation of a region.";
            return true;
        }
    }
}
