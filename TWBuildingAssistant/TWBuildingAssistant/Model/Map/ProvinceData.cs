namespace TWBuildingAssistant.Model.Map
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    using TWBuildingAssistant.Model.Climate;
    using TWBuildingAssistant.Model.Religions;
    using TWBuildingAssistant.Model.Resources;

    public class ProvinceData
    {
        private const int RegionsInProvinceCount = 3;

        private const int MinimalDefaultFretility = 1;

        private const int MaximalDefaultFertility = 6;

        private readonly RegionData[] regions;

        private int currentFertilityDrop;

        public ProvinceData(XElement element, IFertilityDropTracker fertilityDropTracker, IParser<IReligion> religionParser, IParser<IResource> resourceParser, IParser<IClimate> climateParser)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (fertilityDropTracker == null)
            {
                throw new ArgumentNullException(nameof(fertilityDropTracker));
            }

            if (religionParser == null)
            {
                throw new ArgumentNullException(nameof(religionParser));
            }

            if (resourceParser == null)
            {
                throw new ArgumentNullException(nameof(resourceParser));
            }

            if (climateParser == null)
            {
                throw new ArgumentNullException(nameof(climateParser));
            }

            if (!ValidateElement(element, climateParser, religionParser, resourceParser, out var message))
            {
                throw new FormatException(message);
            }

            this.Name = (string)element.Attribute("n");
            this.DefaultFertility = (int)element.Attribute("f");
            this.Climate = climateParser.Parse((string)element.Attribute("c"));
            this.Traditions = new ProvinceTraditions(element.Element("traditions"), religionParser);
            var regionElements = from XElement regionElement in element.Elements() where regionElement.Name == "region" select regionElement;
            this.regions = new RegionData[RegionsInProvinceCount];
            var whichRegion = 0;
            foreach (var regionElement in regionElements)
            {
                this.regions[whichRegion] = new RegionData(regionElement, !Convert.ToBoolean(whichRegion), resourceParser);
                ++whichRegion;
            }

            this.currentFertilityDrop = fertilityDropTracker.FertilityDrop;
            fertilityDropTracker.FertilityDropChanged += (sender, e) => { this.currentFertilityDrop = sender.FertilityDrop; };
        }

        public int RegionsCount => RegionsInProvinceCount;

        public int Fertility
        {
            get
            {
                if (this.DefaultFertility - this.currentFertilityDrop < 0)
                {
                    return 0;
                }

                return this.DefaultFertility - this.currentFertilityDrop;
            }
        }

        public string Name { get; }

        public int DefaultFertility { get; }

        public IClimate Climate { get; }

        public ProvinceTraditions Traditions { get; }

        public RegionData this[int whichRegion] => this.regions[whichRegion];

        public static bool ValidateElement(XElement element, IParser<IClimate> climateParser, IParser<IReligion> religionParser, IParser<IResource> resourceParser, out string message)
        {
            if (element.Attribute("n") == null)
            {
                message = "XML element lacks 'n' attribute.";
                return false;
            }

            if (element.Attribute("f") == null)
            {
                message = "XML element lacks 'f' attribute (name: " + (string)element.Attribute("n") + ").";
                return false;
            }

            if (element.Attribute("c") == null)
            {
                message = "XML element lacks 'c' attribute (name: " + (string)element.Attribute("n") + ").";
                return false;
            }

            if (!int.TryParse((string)element.Attribute("f"), out var fertility))
            {
                message = "XML element's 'f' attribute is not an integer value (name: " + (string)element.Attribute("n") + ").";
                return false;
            }

            if (fertility < MinimalDefaultFretility || fertility > MaximalDefaultFertility)
            {
                message = "XML element's 'f' attribute is out of range (name: " + (string)element.Attribute("n") + ").";
                return false;
            }

            if (climateParser.Parse((string)element.Attribute("c")) == null)
            {
                message = "XML element's 'c' attribute is not a climate name (name: " + (string)element.Attribute("n") + ").";
                return false;
            }

            if (element.Elements().Count() != RegionsInProvinceCount + 1)
            {
                message = "XML element has incorrect sub-elements count (name: " + (string)element.Attribute("n") + ").";
                return false;
            }

            if (!ProvinceTraditions.ValidateElement(element.Element("traditions"), religionParser, out string traditionsMessage))
            {
                message = "XML element contains invalid traditions desription (name: " + (string)element.Attribute("n") + "): " + traditionsMessage;
                return false;
            }

            foreach (var subelement in from XElement regionElement in element.Elements() where regionElement.Name == "region" select regionElement)
            {
                if (RegionData.ValidateElement(subelement, resourceParser, out var regionMessage))
                {
                    continue;
                }

                message = "XML element contains invalid region desription(s) (name: " + (string)element.Attribute("n") + "): " + regionMessage;
                return false;
            }

            message = "XML element is a valid representation of a province.";
            return true;
        }
    }
}