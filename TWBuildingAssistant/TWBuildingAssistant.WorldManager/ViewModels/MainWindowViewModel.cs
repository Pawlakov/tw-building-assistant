namespace TWBuildingAssistant.WorldManager.ViewModels
{
    using System;
    using System.Reactive.Linq;
    using ReactiveUI;
    using TWBuildingAssistant.Data.Sqlite;
    using TWBuildingAssistant.Data.Sqlite.Entities;
    using TWBuildingAssistant.WorldManager.ViewModels.Resources;

    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase content;

        public MainWindowViewModel(DatabaseContext context)
        {
            this.Content = this.List = new ListViewModel(context.Resources);
        }

        public ViewModelBase Content
        {
            get => this.content;
            private set => this.RaiseAndSetIfChanged(ref this.content, value);
        }

        public ListViewModel List { get; }

        public void AddItem()
        {
            var vm = new AddViewModel();

            Observable.Merge(
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
                });

            this.Content = vm;
        }
    }
}