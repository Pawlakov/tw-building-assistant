namespace TWBuildingAssistant.Model.Map
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using TWBuildingAssistant.Model.Climate;
    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Religions;
    using TWBuildingAssistant.Model.Resources;

    using Unity;

    public delegate void ProvinceChangedHandler(ProvincesManager sender, EventArgs e);

    public class ProvincesManager : IFertilityDropTracker
    {
        private const string SourceFile = @"Model\Map\twa_map.xml";

        private const int MinimalFertilityDrop = 0;

        private const int MaximalFertilityDrop = 4;

        private readonly ProvinceData[] provinces;

        private readonly XElement[] elements;

        public ProvincesManager(IUnityContainer resolver)
        {
            this.ReligionParser = resolver.Resolve<Parser<IReligion>>();
            this.ResourceParser = resolver.Resolve<Parser<IResource>>();
            this.ClimateParser = resolver.Resolve<Parser<IClimate>>();

            if (!Validate(this.ClimateParser, this.ReligionParser, this.ResourceParser, out var message))
            {
                throw new FormatException("Cannot create information on provinces: " + message);
            }

            var sourceDocument = XDocument.Load(SourceFile);
            this.elements = (from XElement element in sourceDocument.Root.Elements() select element).ToArray();
            this.provinces = new ProvinceData[this.elements.Count()];
        }

        public event FertilityDropChangedEventHandler FertilityDropChanged;

        public event ProvinceChangedHandler ProvinceChanged;

        public int FertilityDrop { get; private set; }

        public ProvinceData Province
        {
            get
            {
                if (this.provinces[this.ProvinceIndex] != null)
                {
                    return this.provinces[this.ProvinceIndex];
                }

                this.provinces[this.ProvinceIndex] = new ProvinceData(
                this.elements[this.ProvinceIndex],
                this,
                this.ReligionParser,
                this.ResourceParser,
                this.ClimateParser);

                return this.provinces[this.ProvinceIndex];
            }
        }

        public int ProvincesCount => this.provinces.Length;

        public IEnumerable<KeyValuePair<int, string>> AllProvincesNames
        {
            get
            {
                var result = new List<KeyValuePair<int, string>>(this.ProvincesCount);
                for (var whichProvince = 0; whichProvince < this.ProvincesCount; ++whichProvince)
                {
                    result.Add(
                    new KeyValuePair<int, string>(whichProvince, (string)this.elements[whichProvince].Attribute("n")));
                }

                return result;
            }
        }

        public IProvincialEffect Effect => this.Province.Climate.Effect;

        private Parser<IReligion> ReligionParser { get; }

        private Parser<IResource> ResourceParser { get; }

        private Parser<IClimate> ClimateParser { get; }

        private int ProvinceIndex { get; set; } = -1;

        public static bool Validate(Parser<IClimate> climateParser, Parser<IReligion> religionParser, Parser<IResource> resourceParser, out string message)
        {
            if (!File.Exists(SourceFile))
            {
                message = "Corresponding file not found.";
                return false;
            }

            var document = XDocument.Load(SourceFile);
            if (document.Root == null || !document.Root.Elements().Any())
            {
                message = "Corresponding file is incomplete.";
                return false;
            }

            foreach (var element in document.Root.Elements())
            {
                if (ProvinceData.ValidateElement(element, climateParser, religionParser, resourceParser, out var elementMessage))
                {
                    continue;
                }

                message = "One of XML elements is invalid: " + elementMessage;
                return false;
            }

            message = "Information on provinces is valid and complete.";
            return true;
        }

        public void ChangeFertilityDrop(int fertilityDrop)
        {
            if (fertilityDrop < MinimalFertilityDrop || fertilityDrop > MaximalFertilityDrop)
            {
                throw new ArgumentOutOfRangeException(
                nameof(fertilityDrop),
                fertilityDrop,
                "The fertility drop is out of range.");
            }

            this.FertilityDrop = fertilityDrop;
            this.OnFertilityDropChanged();
        }

        public void ChangeProvince(int whichProvince)
        {
            if (whichProvince < 0 || whichProvince > (this.ProvincesCount - 1))
            {
                throw new ArgumentOutOfRangeException(
                nameof(whichProvince),
                whichProvince,
                "The index of province is out of range.");
            }

            this.ProvinceIndex = whichProvince;
            this.OnProvinceChangedChanged();
        }

        private void OnFertilityDropChanged()
        {
            this.FertilityDropChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnProvinceChangedChanged()
        {
            this.ProvinceChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}