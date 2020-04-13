namespace TWBuildingAssistant.WorldManager.ViewModels.Resources
{
    using System.Reactive;
    using ReactiveUI;
    using TWBuildingAssistant.Data.Sqlite.Model;

    public class AddViewModel : ViewModelBase
    {
        private string description;

        public AddViewModel()
        {
            var okEnabled = this.WhenAnyValue(
                x => x.Description,
                x => !string.IsNullOrWhiteSpace(x));

            this.Ok = ReactiveCommand.Create(
                () => new Resource { Name = this.Description },
                okEnabled);
            this.Cancel = ReactiveCommand.Create(() => { });
        }

        public string Description
        {
            get => this.description;
            set => this.RaiseAndSetIfChanged(ref this.description, value);
        }

        public ReactiveCommand<Unit, Resource> Ok { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }
    }
}