namespace TWBuildingAssistant.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using TWBuildingAssistant.Model;

    public class ViewModelSettingsWindow : ViewModelWindow
    {
        private object selectedReligion;

        private object selectedProvince;

        private object selectedFaction;

        private object selectedFertilityDrop;

        private object selectedTechnologyTier;

        private bool useLegacy = true;

        public ViewModelSettingsWindow()
        {
            var world = World.GetWorld();
            this.Religions = new ObservableCollection<KeyValuePair<int, string>>(world.Religions);
            this.SelectedReligion = this.Religions[0];
            this.Provinces = new ObservableCollection<KeyValuePair<int, string>>(world.Provinces);
            this.SelectedProvince = this.Provinces[0];
            this.Factions = new ObservableCollection<KeyValuePair<int, string>>(world.Factions);
            this.SelectedFaction = this.Factions[0];
            this.FertilityDrops = new ObservableCollection<int> { 0, 1, 2, 3, 4 };
            this.SelectedFertilityDrop = this.FertilityDrops[0];
            this.Weathers = new ObservableCollection<CheckedListItem<KeyValuePair<int, string>>>(world.Weathers.Select(x => new CheckedListItem<KeyValuePair<int, string>>(x, true)));
            this.TechnologyTiers = new ObservableCollection<int> { 0, 1, 2, 3, 4 };
            this.SelectedTechnologyTier = this.TechnologyTiers[0];
            this.SubmitCommand = new RelayCommand(this.Submit);
        }

        public ObservableCollection<KeyValuePair<int, string>> Religions { get; set; }

        public object SelectedReligion
        {
            get => this.selectedReligion;
            set
            {
                if (value == this.selectedReligion)
                {
                    return;
                }

                this.selectedReligion = value;
                this.OnPropertyChanged(nameof(this.SelectedReligion));
            }
        }

        public ObservableCollection<KeyValuePair<int, string>> Provinces { get; set; }

        public object SelectedProvince
        {
            get => this.selectedProvince;
            set
            {
                if (value == this.selectedProvince)
                {
                    return;
                }

                this.selectedProvince = value;
                this.OnPropertyChanged(nameof(this.SelectedProvince));
            }
        }

        public ObservableCollection<KeyValuePair<int, string>> Factions { get; set; }

        public object SelectedFaction
        {
            get => this.selectedFaction;
            set
            {
                if (value == this.selectedFaction)
                {
                    return;
                }

                this.selectedFaction = value;
                this.OnPropertyChanged(nameof(this.SelectedFaction));
            }
        }

        public ObservableCollection<int> FertilityDrops { get; set; }

        public object SelectedFertilityDrop
        {
            get => this.selectedFertilityDrop;
            set
            {
                if (value == this.selectedFertilityDrop)
                {
                    return;
                }

                this.selectedFertilityDrop = value;
                this.OnPropertyChanged(nameof(this.SelectedFertilityDrop));
            }
        }

        public ObservableCollection<CheckedListItem<KeyValuePair<int, string>>> Weathers { get; set; }

        public ObservableCollection<int> TechnologyTiers { get; set; }

        public object SelectedTechnologyTier
        {
            get => this.selectedTechnologyTier;
            set
            {
                if (value == this.selectedTechnologyTier)
                {
                    return;
                }

                this.selectedTechnologyTier = value;
                this.OnPropertyChanged(nameof(this.SelectedTechnologyTier));
            }
        }

        public bool UseLegacy
        {
            get => this.useLegacy;
            set
            {
                if (value == this.useLegacy)
                {
                    return;
                }

                this.useLegacy = value;
                this.OnPropertyChanged(nameof(this.UseLegacy));
            }
        }

        public RelayCommand SubmitCommand { get; }

        private void Submit(object paramter)
        {
            var kit = World.GetWorld().AssembleSimulationKit(this.CollectSettings());
            var simulationWindow = new View.SimulationWindow(kit);
            simulationWindow.Show();
            this.OnCloseWindow();
        }

        private WorldSettings CollectSettings()
        {
            return new WorldSettings()
            {
                StateReligionIndex = ((KeyValuePair<int, string>)this.SelectedReligion).Key,
                ProvinceIndex = ((KeyValuePair<int, string>)this.SelectedProvince).Key,
                FactionIndex = ((KeyValuePair<int, string>)this.SelectedFaction).Key,
                FertilityDrop = (int)this.SelectedFertilityDrop,
                ConsideredWeathers = this.Weathers.Where(x => x.IsChecked).Select(x => x.Item.Key),
                DesiredTechnologyLevelIndex = (int)this.SelectedTechnologyTier,
                UseLegacyTechnologies = this.UseLegacy
            };
        }
    }
}