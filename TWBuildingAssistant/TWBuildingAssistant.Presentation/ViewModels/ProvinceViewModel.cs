namespace TWBuildingAssistant.Presentation.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using ReactiveUI;
    using TWBuildingAssistant.Model;

    public class ProvinceViewModel : ViewModel
    {
        private readonly Province province;

        private string performance;

        public ProvinceViewModel(Province province)
        {
            this.province = province;
            this.ProvinceName = province.Name;
            this.Regions = new ObservableCollection<RegionViewModel>();
            foreach (var region in province.Regions)
            {
                var newRegion = new RegionViewModel(province, region);
                foreach (var slot in newRegion.Slots)
                {
                    slot.PropertyChanged += (sender, args) => this.SetPerformanceDisplay();
                }

                this.Regions.Add(newRegion);
            }

            this.SetPerformanceDisplay();
        }

        public string ProvinceName { get; }

        public ObservableCollection<RegionViewModel> Regions { get; }

        public string Performance
        {
            get => this.performance;
            set => this.RaiseAndSetIfChanged(ref this.performance, value);
        }

        private void SetPerformanceDisplay()
        {
            var state = this.province.State;
            var builder = new StringBuilder();
            builder.AppendLine($"Sanitation: {string.Join("/", state.Sanitation.Select(x => x.ToString()))}");
            builder.AppendLine($"Food: {state.Food}");
            builder.AppendLine($"Public Order: {state.PublicOrder}");
            builder.AppendLine($"Relgious Osmosis: {state.ReligiousOsmosis}");
            builder.AppendLine($"Research Rate: +{state.ResearchRate}%");
            builder.AppendLine($"Growth: {state.Growth}");
            builder.AppendLine($"Wealth: {state.Wealth}");
            this.Performance = builder.ToString();
        }
    }
}