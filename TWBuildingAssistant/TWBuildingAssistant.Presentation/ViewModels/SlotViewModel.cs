namespace TWBuildingAssistant.Presentation.ViewModels
{
    using System.Collections.ObjectModel;
    using ReactiveUI;
    using TWBuildingAssistant.Model;

    public class SlotViewModel : ViewModel
    {
        private readonly BuildingSlot slot;

        private BuildingLevel selectedBuilding;

        public SlotViewModel(Province province, Region region, BuildingSlot slot)
        {
            this.slot = slot;
            var buildings = province.Owner.GetBuildingLevelsForSlot(province, region, slot);
            this.Buildings = new ObservableCollection<BuildingLevel>(buildings);
            this.selectedBuilding = slot.Building ?? this.Buildings[0];
            slot.Building = this.selectedBuilding;
        }

        public ObservableCollection<BuildingLevel> Buildings { get; }

        public BuildingLevel SelectedBuilding
        {
            get => this.selectedBuilding;
            set
            {
                this.slot.Building = value;
                this.RaiseAndSetIfChanged(ref this.selectedBuilding, value);
            }
        }
    }
}