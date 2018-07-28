namespace TWBuildingAssistant.Model.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class ProvinceData
    {
        private const int _regionsInProvinceCount = 3;

        private const int _minimalDefaultFretility = 1;

        private const int _maximalDefaultFertility = 6;

        public ProvinceData(XElement element, IFertilityDropTracker fertilityDropTracker, IReligionParser religionParser, IResourceParser resourceParser, IClimateParser climateParser, IStateReligionTracker stateReligionTracker)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            if (fertilityDropTracker == null)
                throw new ArgumentNullException("owner");
            if (religionParser == null)
                throw new ArgumentNullException("religionParser");
            if (resourceParser == null)
                throw new ArgumentNullException("resourceParser");
            if (climateParser == null)
                throw new ArgumentNullException("climateParser");
            if (!ValidateElement(element, climateParser, religionParser, resourceParser, out string message))
                throw new FormatException(message);
            //
            Name = (string)element.Attribute("n");
            DefaultFertility = (int)element.Attribute("f");
            Climate = climateParser.Parse((string)element.Attribute("c"));
            //
            Traditions = new ProvinceTraditions(element.Element("traditions"), religionParser, stateReligionTracker);
            IEnumerable<XElement> regionElements = from XElement regionElement in element.Elements() where regionElement.Name == "region" select regionElement;
            _regions = new RegionData[_regionsInProvinceCount];
            int whichRegion = 0;
            foreach (XElement regionElement in regionElements)
            {
                _regions[whichRegion] = new RegionData(regionElement, !Convert.ToBoolean(whichRegion), resourceParser);
                ++whichRegion;
            }
            _currentFertilityDrop = fertilityDropTracker.FertilityDrop;
            fertilityDropTracker.FertilityDropChanged += (IFertilityDropTracker sender, EventArgs e) => { _currentFertilityDrop = sender.FertilityDrop; };
        }

        public RegionData this[int whichRegion]
        {
            get { return _regions[whichRegion]; }
        }

        public int RegionsCount
        {
            get { return _regionsInProvinceCount; }
        }

        public int Fertility
        {
            get
            {
                if (DefaultFertility - _currentFertilityDrop < 0)
                    return 0;
                return DefaultFertility - _currentFertilityDrop;
            }
        }

        public string Name { get; }

        public int DefaultFertility { get; }

        public ClimateAndWeather.Climate Climate { get; }

        public ProvinceTraditions Traditions { get; }

        private readonly RegionData[] _regions;

        private int _currentFertilityDrop;

        public static bool ValidateElement(XElement element, IClimateParser climateParser, IReligionParser religionParser, IResourceParser resourceParser, out string message)
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
            if (!Int32.TryParse((string)element.Attribute("f"), out int irrelevant))
            {
                message = "XML element's 'f' attribute is not an integer value (name: " + (string)element.Attribute("n") + ").";
                return false;
            }
            if (irrelevant < _minimalDefaultFretility || irrelevant > _maximalDefaultFertility)
            {
                message = "XML element's 'f' attribute is out of range (name: " + (string)element.Attribute("n") + ").";
                return false;
            }
            if (climateParser.Parse((string)element.Attribute("c")) == null)
            {
                message = "XML element's 'c' attribute is not a climate name (name: " + (string)element.Attribute("n") + ").";
                return false;
            }
            if (element.Elements().Count() != _regionsInProvinceCount + 1)
            {
                message = "XML element has incorrect sub-elements count (name: " + (string)element.Attribute("n") + ").";
                return false;
            }
            if (!ProvinceTraditions.ValidateElement(element.Element("traditions"), religionParser, out string traditionsMessage))
            {
                message = "XML element contains invalid traditions desription (name: " + (string)element.Attribute("n") + "): " + traditionsMessage;
                return false;
            }
            foreach (XElement subelement in from XElement regionElement in element.Elements() where regionElement.Name == "region" select regionElement)
            {
                if (!RegionData.ValidateElement(subelement, resourceParser, out string regionMessage))
                {
                    message = "XML element contains invalid region desription(s) (name: " + (string)element.Attribute("n") + "): " + regionMessage;
                    return false;
                }
            }
            message = "XML element is a valid representation of a province.";
            return true;
        }
    }
}