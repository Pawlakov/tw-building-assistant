namespace TWBuildingAssistant.Old.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public class Slot : INotifyPropertyChanged
    {
        private object selectedBuilding;

        public Slot(Model.BuildingSlot slot)
        {
            this.Buildings = new ObservableCollection<KeyValuePair<int, string>>();
            this.Buildings.Add(new KeyValuePair<int, string>(0, "null"));
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
                this.OnPropertyChanged(nameof(this.SelectedBuilding));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}