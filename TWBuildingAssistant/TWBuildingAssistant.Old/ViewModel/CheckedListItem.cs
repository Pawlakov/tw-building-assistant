namespace TWBuildingAssistant.Old.ViewModel
{
    using System.ComponentModel;

    public class CheckedListItem<T> : INotifyPropertyChanged
    {
        private bool isChecked;

        private T item;

        public CheckedListItem()
        {
            this.item = default(T);
            this.isChecked = false;
        }

        public CheckedListItem(T item, bool isChecked = false)
        {
            this.item = item;
            this.isChecked = isChecked;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public T Item
        {
            get => this.item;
            set
            {
                this.item = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
            }
        }

        public bool IsChecked
        {
            get => this.isChecked;
            set
            {
                this.isChecked = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }
    }
}