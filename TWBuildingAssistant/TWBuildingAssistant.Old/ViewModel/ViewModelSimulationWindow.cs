namespace TWBuildingAssistant.Old.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Windows.Media;
    using TWBuildingAssistant.Model;

    public class ViewModelSimulationWindow : ViewModelWindow
    {
        private readonly Province province;

        private string performance = string.Empty;

        private SolidColorBrush performanceColor;

        public ViewModelSimulationWindow(Province province)
        {
            this.province = province;
            this.ProvinceName = province.Name;
            this.Regions = new ObservableCollection<Region>();
            foreach (var region in province.Regions)
            {
                var newRegion = new Region(region);
                foreach (var slot in newRegion.Slots)
                {
                    //slot.PropertyChanged += (sender, args) => this.SetPerformanceDisplay(this.simulation.CurrentPerformance());
                }
                
                this.Regions.Add(newRegion);
            }

            //this.SetPerformanceDisplay(this.simulation.CurrentPerformance());
        }

        public string ProvinceName { get; }

        public ObservableCollection<Region> Regions { get; }

        //public string Performance
        //{
        //    get => this.performance;
        //    set
        //    {
        //        if (this.performance.Equals(value))
        //        {
        //            return;
        //        }

        //        this.performance = value;
        //        this.OnPropertyChanged(nameof(this.Performance));
        //    }
        //}

        //public SolidColorBrush PerformanceColor
        //{
        //    get => this.performanceColor;
        //    set
        //    {
        //        if (Equals(this.performanceColor, value))
        //        {
        //            return;
        //        }

        //        this.performanceColor = value;
        //        this.OnPropertyChanged(nameof(this.PerformanceColor));
        //    }
        //}

        //private void SetPerformanceDisplay(Model.SimulationKit.ProvinceState currentPerformance)
        //{
        //    var sanitationString = currentPerformance.Sanitation[0].ToString();
        //    for (var whichRegion = 1; whichRegion < currentPerformance.Sanitation.Length; whichRegion++)
        //    {
        //        sanitationString = $"{sanitationString}/{currentPerformance.Sanitation[whichRegion]}";
        //    }

        //    var builder = new StringBuilder();
        //    builder.AppendLine($"Sanitation: {sanitationString}");
        //    builder.AppendLine($"Food: {currentPerformance.Food}");
        //    builder.AppendLine($"Public Order: {currentPerformance.PublicOrder}");
        //    builder.AppendLine($"Relgious Osmosis: {currentPerformance.ReligiousOsmosis}");
        //    builder.AppendLine($"Fertility: {currentPerformance.Fertility}");
        //    builder.AppendLine($"Research Rate: +{currentPerformance.ResearchRate}%");
        //    builder.AppendLine($"Growth: {currentPerformance.Growth}");
        //    builder.AppendLine($"Wealth: {currentPerformance.Wealth}");
        //    this.Performance = builder.ToString();

        //    this.PerformanceColor = currentPerformance.Conflict ? Brushes.Red : Brushes.Black;
        //}
    }
}