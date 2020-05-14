namespace TWBuildingAssistant.Presentation.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using ReactiveUI;
    using TWBuildingAssistant.Model;

    public class SlotViewModel : ViewModel
    {
        private readonly BuildingSlot slot;

        private readonly Region region;

        private readonly Province province;

        private BuildingLevel selectedBuilding;

        private bool seek;

        public SlotViewModel(Province province, Region region, BuildingSlot slot)
        {
            this.slot = slot;
            this.region = region;
            this.province = province;
            this.Buildings = new ObservableCollection<BuildingLevel>();
            this.selectedBuilding = slot.Building;
            this.seek = false;
        }

        public event EventHandler BuildingChanged;

        public BuildingSlot Slot => this.slot;

        public ObservableCollection<BuildingLevel> Buildings { get; }

        public BuildingLevel SelectedBuilding
        {
            get => this.selectedBuilding;
            set
            {
                this.slot.Building = value;
                this.RaiseAndSetIfChanged(ref this.selectedBuilding, value);
                this.BuildingChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool Seek
        {
            get => this.seek;
            set
            {
                this.RaiseAndSetIfChanged(ref this.seek, value);
            }
        }

        public void UpdateBuildings()
        {
            var buildings = this.province.Owner.GetBuildingLevelsForSlot(this.province, this.region, this.slot);
            foreach (var building in this.Buildings.ToList())
            {
                if (building != this.selectedBuilding)
                {
                    this.Buildings.Remove(building);
                }
            }

            foreach (var building in buildings)
            {
                if (!this.Buildings.Contains(building))
                {
                    this.Buildings.Add(building);
                }
            }

            if (this.selectedBuilding == null)
            {
                this.selectedBuilding = this.Buildings.First();
            }

            this.slot.Building = this.selectedBuilding;
        }
    }
}