namespace TWBuildingAssistant.WorldManager.ViewModels.Resources;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using TWBuildingAssistant.Data.Sqlite.Entities;

public class ListViewModel : ViewModelBase
{
    public ListViewModel(IEnumerable<Resource> items)
    {
        this.Items = new ObservableCollection<Resource>(items);
    }

    public ObservableCollection<Resource> Items { get; }
}