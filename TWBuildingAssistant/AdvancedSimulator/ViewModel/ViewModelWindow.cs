namespace TWBuildingAssistant.ViewModel
{
    using System;
    using System.ComponentModel;

    public class ViewModelWindow : INotifyPropertyChanged
    {
        public event EventHandler CloseWindow;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnCloseWindow()
        {
            this.CloseWindow?.Invoke(this, EventArgs.Empty);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}