namespace TWBuildingAssistant.WorldManager.ViewModels;

using TWBuildingAssistant.Data.Sqlite;
using TWBuildingAssistant.Data.Sqlite.Entities;
using TWBuildingAssistant.WorldManager.ViewModels.Resources;

public class MainWindowViewModel 
    : ViewModel
{
    private ViewModel content;

    public MainWindowViewModel(DatabaseContext context)
    {
        this.Content = this.List = new ListViewModel(context.Resources);
    }

    public ViewModel Content
    {
        get => this.content;
        private set
        {
            if (this.content != value)
            {
                this.content = value;
                this.OnPropertyChanged(nameof(this.Content));
            }
        }
    }

    public ListViewModel List { get; }

    public void AddItem()
    {
        var vm = new AddViewModel();

        /*Observable.Merge(
            vm.Ok,
            vm.Cancel.Select(_ => (Resource)null))
            .Take(1)
            .Subscribe(model =>
            {
                if (model != null)
                {
                    this.List.Items.Add(model);
                }

                this.Content = this.List;
            });*/

        this.Content = vm;
    }
}