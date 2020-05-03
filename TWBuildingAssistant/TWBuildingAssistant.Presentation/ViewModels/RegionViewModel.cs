namespace TWBuildingAssistant.Presentation.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using TWBuildingAssistant.Model;

    public class RegionViewModel : ViewModel
    {
        public RegionViewModel(Province province, Region region)
        {
            var count = region.Slots.Count();
            this.Slots = new ObservableCollection<SlotViewModel>();
            foreach (var slot in region.Slots)
            {
                this.Slots.Add(new SlotViewModel(province, region, slot));
            }

            this.Name = region.Name;
        }

        public ObservableCollection<SlotViewModel> Slots { get; }

        public string Name { get; }
    }
}