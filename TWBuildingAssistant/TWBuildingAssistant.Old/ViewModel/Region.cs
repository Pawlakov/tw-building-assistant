namespace TWBuildingAssistant.Old.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    public class Region : INotifyPropertyChanged
    {
        private string name = string.Empty;

        public Region(TWBuildingAssistant.Model.Region region)
        {
            var count = region.Slots.Count();
            this.Slots = new ObservableCollection<Slot>();
            foreach (var slot in region.Slots)
            {
                this.Slots.Add(new Slot(slot));
            }

            this.Name = region.Name;
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