namespace TWBuildingAssistant.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public class Slot : INotifyPropertyChanged
    {
        private object selectedBuilding;

        private readonly int whichRegion;

        private readonly int whichSlot;

        private readonly Model.SimulationKit kit;

        public Slot(int whichRegion, int whichSlot, Model.SimulationKit kit)
        {
            this.kit = kit;
            this.whichRegion = whichRegion;
            this.whichSlot = whichSlot;
            this.Buildings = new ObservableCollection<KeyValuePair<int, string>>(this.kit.GetAvailableBuildingsAt(whichRegion, whichSlot));
            this.SelectedBuilding = this.Buildings[0];
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<KeyValuePair<int, string>> Buildings { get; }

        public object SelectedBuilding
        {
            get => this.selectedBuilding;
            set
            {
                if (this.selectedBuilding == value)
                {
                    return;
                }

                this.selectedBuilding = value;
                this.kit.SetBuildingAt(this.whichRegion, this.whichSlot, this.Buildings.IndexOf((KeyValuePair<int, string>)value));
                this.OnPropertyChanged(nameof(this.SelectedBuilding));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}