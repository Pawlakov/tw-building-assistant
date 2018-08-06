namespace TWBuildingAssistant.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    public class Region : INotifyPropertyChanged
    {
        private string name = string.Empty;

        public Region(Model.SimulationKit kit, int whichRegion)
        {
            var count = kit.GetSlotsCount(whichRegion);
            this.Slots = new ObservableCollection<Slot>();
            for (var whichSlot = 0; whichSlot < count; ++whichSlot)
            {
                this.Slots.Add(new Slot(whichRegion, whichSlot, kit));
            }

            this.Name = kit.RegionName(whichRegion);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Slot> Slots { get; }

        public string Name
        {
            get => this.name;
            set
            {
                if (this.Name.Equals(value))
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged(nameof(this.Name));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}