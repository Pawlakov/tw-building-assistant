namespace TWBuildingAssistant.Model.Map
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    public class ProvincesManager : IFertilityDropTracker
    {
        public ProvincesManager(IReligionParser religionParser, IResourceParser resourceParser, IStateReligionTracker stateReligionTracker)
        {
            ClimateManager = new ClimateAndWeather.ClimateManager();
            if (!Validate(ClimateManager, religionParser, resourceParser, out string message))
                throw new FormatException("Cannot create information on provinces: " + message);
            ReligionParser = religionParser;
            ResourceParser = resourceParser;
            StateReligionTracker = stateReligionTracker;
            XDocument sourceDocument = XDocument.Load(_sourceFile);
            _elements = (from XElement element in sourceDocument.Root.Elements() select element).ToArray();
            _provinces = new ProvinceData[_elements.Count()];
        }

        public event FertilityDropChangedEventHandler FertilityDropChanged;

        public event ProvinceChangedHandler ProvinceChanged;

        private const string _sourceFile = "Model\\Map\\twa_map.xml";

        private const int _minimalFertilityDrop = 0;

        private const int _maximalFertilityDrop = 4;

        private ClimateAndWeather.ClimateManager ClimateManager { get; }

        private IReligionParser ReligionParser { get; }

        private IResourceParser ResourceParser { get; }

        private IStateReligionTracker StateReligionTracker { get; }

        private readonly ProvinceData[] _provinces;

        private readonly XElement[] _elements;

        public int FertilityDrop { get; private set; }

        private int ProvinceIndex { get; set; } = -1;

        public ProvinceData Province
        {
            get
            {
                if (_provinces[ProvinceIndex] == null)
                    _provinces[ProvinceIndex] = new ProvinceData(_elements[ProvinceIndex], this, ReligionParser, ResourceParser, ClimateManager, StateReligionTracker);
                return _provinces[ProvinceIndex];
            }
        }

        public int ProvincesCount
        {
            get { return _provinces.Length; }
        }

        public void ChangeFertilityDrop(int fertilityDrop)
        {
            if (fertilityDrop < _minimalFertilityDrop || fertilityDrop > _maximalFertilityDrop)
                throw new ArgumentOutOfRangeException("fertilityDrop", fertilityDrop, "The fertility drop is out of range.");
            FertilityDrop = fertilityDrop;
            OnFertilityDropChanged();
        }

        public void ChangeProvince(int whichProvince)
        {
            if (whichProvince < 0 || whichProvince > (ProvincesCount - 1))
                throw new ArgumentOutOfRangeException("whichProvince", whichProvince, "The index of province is out of range.");
            ProvinceIndex = whichProvince;
            OnProvinceChangedChanged();
        }

        public void ChangeWorstCaseWeather(ClimateAndWeather.Weather whichWeather)
        {
            ClimateManager.ChangeWorstCaseWeather(whichWeather);
        }

        public IEnumerable<KeyValuePair<int, string>> AllProvincesNames
        {
            get
            {
                List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>(ProvincesCount);
                for (int whichProvince = 0; whichProvince < ProvincesCount; ++whichProvince)
                    result.Add(new KeyValuePair<int, string>(whichProvince, (string)_elements[whichProvince].Attribute("n")));
                return result;
            }
        }

        public int PublicOrder
        {
            get
            {
                return Province.Climate.PublicOrder;
            }
        }

        public int Food
        {
            get
            {
                return Province.Climate.Food;
            }
        }

        public int Sanitation
        {
            get
            {
                return Province.Climate.Sanitation;
            }
        }

        public int ReligiousOsmosis
        {
            get
            {
                return 0;
            }
        }

        public int ReligiousInfluence
        {
            get
            {
                return 0;
            }
        }

        public int ResearchRate
        {
            get
            {
                return 0;
            }
        }

        public int Growth
        {
            get
            {
                return 0;
            }
        }

        public int Fertility
        {
            get
            {
                return Province.Fertility;
            }
        }

        public IEnumerable<Effects.WealthBonus> Bonuses
        {
            get
            {
                return new List<Effects.WealthBonus>();
            }
        }

        private void OnFertilityDropChanged()
        {
            FertilityDropChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnProvinceChangedChanged()
        {
            ProvinceChanged?.Invoke(this, EventArgs.Empty);
        }

        public static bool Validate(IClimateParser climateParser, IReligionParser religionParser, IResourceParser resourceParser, out string message)
        {
            XDocument document;
            if (!File.Exists(_sourceFile))
            {
                message = "Corresponding file not found.";
                return false;
            }
            document = XDocument.Load(_sourceFile);
            if (document.Root == null || document.Root.Elements().Count() < 1)
            {
                message = "Corresponding file is incomplete.";
                return false;
            }
            foreach (XElement element in document.Root.Elements())
                if (!ProvinceData.ValidateElement(element, climateParser, religionParser, resourceParser, out string elementMessage))
                {
                    message = "One of XML elements is invalid: " + elementMessage;
                    return false;
                }
            message = "Information on provinces is valid and complete.";
            return true;
        }
    }

    public delegate void ProvinceChangedHandler(ProvincesManager sender, EventArgs e);
}