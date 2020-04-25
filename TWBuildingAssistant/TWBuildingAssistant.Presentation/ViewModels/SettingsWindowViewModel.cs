namespace TWBuildingAssistant.Presentation.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Reactive;
    using ReactiveUI;
    using TWBuildingAssistant.Model;
    using TWBuildingAssistant.Presentation.Views;

    public class SettingsWindowViewModel : WindowViewModel
    {
        private readonly World world;

        private Religion selectedReligion;

        private Province selectedProvince;

        private Faction selectedFaction;

        public SettingsWindowViewModel()
        {
            this.world = new World();
            this.Religions = new ObservableCollection<Religion>(this.world.Religions);
            this.SelectedReligion = this.Religions[0];
            this.Provinces = new ObservableCollection<Province>(this.world.Provinces);
            this.SelectedProvince = this.Provinces[0];
            this.Factions = new ObservableCollection<Faction>(this.world.Factions);
            this.SelectedFaction = this.Factions[0];
            this.SubmitCommand = ReactiveCommand.Create(this.Submit);
        }

        public ObservableCollection<Religion> Religions { get; set; }

        public Religion SelectedReligion
        {
            get => this.selectedReligion;
            set => this.RaiseAndSetIfChanged(ref this.selectedReligion, value);
        }

        public ObservableCollection<Province> Provinces { get; set; }

        public Province SelectedProvince
        {
            get => this.selectedProvince;
            set => this.RaiseAndSetIfChanged(ref this.selectedProvince, value);
        }

        public ObservableCollection<Faction> Factions { get; set; }

        public Faction SelectedFaction
        {
            get => this.selectedFaction;
            set => this.RaiseAndSetIfChanged(ref this.selectedFaction, value);
        }

        public ReactiveCommand<Unit, Unit> SubmitCommand { get; }

        private void Submit()
        {
            this.selectedFaction.StateReligion = this.selectedReligion;
            this.selectedProvince.Owner = this.selectedFaction;
            var simulationWindow = new SimulationWindow();
            var viewModel = new SimulationWindowViewModel(this.selectedProvince);
            simulationWindow.DataContext = viewModel;
            viewModel.CloseWindow += (sender, e) => { simulationWindow.Close(); };
            simulationWindow.Show();
            this.OnCloseWindow();
        }
    }
}