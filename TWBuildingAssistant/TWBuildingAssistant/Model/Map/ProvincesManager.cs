namespace TWBuildingAssistant.Model.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TWBuildingAssistant.Model.Climate;
    using TWBuildingAssistant.Model.Religions;
    using TWBuildingAssistant.Model.Resources;

    using Unity;

    public partial class ProvincesManager : Parser<IProvince>
    {
        public ProvincesManager(IUnityContainer resolver)
        {
            this.Content = resolver.Resolve<ISource>().Provinces.ToArray();
            var religionParser = resolver.Resolve<Parser<IReligion>>();
            var resourceParser = resolver.Resolve<Parser<IResource>>();
            var climateParser = resolver.Resolve<Parser<IClimate>>();
            foreach (var province in this.Content)
            {
                province.SetClimateParser(climateParser);
                province.SetFertilityDropTracker(this);
                foreach (var influence in province.Effect.Influences)
                {
                    influence.SetReligionParser(religionParser);
                }

                foreach (var region in province.Regions)
                {
                    region.SetResourceParser(resourceParser);
                }
            }

            var message = string.Empty;
            if (this.Content.Any(province => !province.Validate(out message)))
            {
                throw new MapException($"One of provinces is not valid ({message}).");
            }
        }

        public IEnumerable<KeyValuePair<int, string>> AllProvincesNames => this.Content.Select(x => new KeyValuePair<int, string>(x.Id, x.Name));
    }

    public partial class ProvincesManager : IFertilityDropTracker
    {
        private const int MinimalFertilityDrop = 0;

        private const int MaximalFertilityDrop = 4;

        public event EventHandler<FertilityDropChangedEventArgs> FertilityDropChanged;

        public int FertilityDrop { get; private set; }

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

        private void OnFertilityDropChanged()
        {
            this.FertilityDropChanged?.Invoke(this, new FertilityDropChangedEventArgs(this, this.FertilityDrop));
        }
    }
}