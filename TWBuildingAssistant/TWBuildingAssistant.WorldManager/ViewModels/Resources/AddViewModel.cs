namespace TWBuildingAssistant.WorldManager.ViewModels.Resources;

using CommunityToolkit.Mvvm.Input;
using TWBuildingAssistant.Data.Sqlite.Entities;

public class AddViewModel : ViewModel
{
    private string description;

    public AddViewModel()
    {
        this.OkCommand = new RelayCommand(this.Ok, this.OkEnabled);
        this.CancelCommand = new RelayCommand(this.Cancel);
    }

    public string Description
    {
        get => this.description;
        set
        {
            if (this.description != value)
            {
                this.description = value;
                this.OnPropertyChanged(nameof(Description));
            }
        }
    }

    public RelayCommand OkCommand { get; init;  }

    public RelayCommand CancelCommand { get; init; }

    private void Ok()
    {
        var resource = new Resource { Name = this.Description };
    }

    private bool OkEnabled()
    {
        return !string.IsNullOrWhiteSpace(this.Description);
    }

    private void Cancel()
    {

    }
}