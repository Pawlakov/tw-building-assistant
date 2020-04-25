namespace TWBuildingAssistant.Presentation.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;

    public class Region : ViewModel
    {
        public Region(Model.Region region)
        {
            var count = region.Slots.Count();
            this.Slots = new ObservableCollection<Slot>();
            foreach (var slot in region.Slots)
            {
                this.Slots.Add(new Slot(slot));
            }

            this.Name = region.Name;
        }

        public ObservableCollection<Slot> Slots { get; }

        public string Name { get; }
    }
}