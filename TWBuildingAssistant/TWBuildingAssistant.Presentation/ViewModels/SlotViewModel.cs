namespace TWBuildingAssistant.Presentation.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using ReactiveUI;
    using TWBuildingAssistant.Model;

    public class SlotViewModel : ViewModel
    {
        private object selectedBuilding;

        public SlotViewModel(BuildingSlot slot)
        {
            this.Buildings = new ObservableCollection<KeyValuePair<int, string>>();
            this.Buildings.Add(new KeyValuePair<int, string>(0, "null"));
            this.SelectedBuilding = this.Buildings[0];
        }

        public ObservableCollection<KeyValuePair<int, string>> Buildings { get; }

        public object SelectedBuilding
        {
            get => this.selectedBuilding;
            set => this.RaiseAndSetIfChanged(ref this.selectedBuilding, value);
        }
    }
}