namespace TWBuildingAssistant.Presentation.ViewModels;

using System.ComponentModel;

public class ViewModel 
    : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void Dispose() 
    { 
    }

    protected void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}