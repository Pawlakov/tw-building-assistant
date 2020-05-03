namespace TWBuildingAssistant.Presentation.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using TWBuildingAssistant.Model;

    public class RegionViewModel : ViewModel
    {
        public RegionViewModel(Province province, Region region)
        {
            this.Slots = new ObservableCollection<SlotViewModel>();
            foreach (var slot in region.Slots)
            {
                this.Slots.Add(new SlotViewModel(province, region, slot));
            }

            foreach (var slotViewModel in this.Slots)
            {
                slotViewModel.BuildingChanged += (sender, args) => this.Slots.ToList().ForEach(x => x.UpdateBuildings());
                slotViewModel.UpdateBuildings();
            }

            this.Name = region.Name;
        }

        public ObservableCollection<SlotViewModel> Slots { get; }

        public string Name { get; }
    }
}