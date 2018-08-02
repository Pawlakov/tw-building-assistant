namespace TWBuildingAssistant.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using TWBuildingAssistant.Model;

    public class ViewModelSettingsWindow : ViewModelWindow
    {
        private World world;

        private object selectedReligion;

        private object selectedProvince;

        private object selectedFaction;

        private object selectedFertilityDrop;

        private object selectedWeather;

        private object selectedTechnologyTier;

        private bool useLegacy = true;

        public ViewModelSettingsWindow()
        {
            this.world = new Model.World();
            this.Religions = new ObservableCollection<KeyValuePair<int, string>>(this.world.Religions);
            this.SelectedReligion = this.Religions[0];
            this.Provinces = new ObservableCollection<KeyValuePair<int, string>>(this.world.Provinces);
            this.SelectedProvince = this.Provinces[0];
            this.Factions = new ObservableCollection<KeyValuePair<int, string>>(this.world.Factions);
            this.SelectedFaction = this.Factions[0];
            this.FertilityDrops = new ObservableCollection<int> { 0, 1, 2, 3, 4 };
            this.SelectedFertilityDrop = this.FertilityDrops[0];
            this.Weathers = new ObservableCollection<string> { "Good", "Normal", "Bad", "Extreme" };
            this.SelectedWeather = this.Weathers[2];
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

        public ObservableCollection<string> Weathers { get; set; }

        public object SelectedWeather
        {
            get => this.selectedWeather;
            set
            {
                if (value == this.selectedWeather)
                {
                    return;
                }

                this.selectedWeather = value;
                this.OnPropertyChanged(nameof(this.SelectedWeather));
            }
        }

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
            var kit = world.AssembleSimulationKit(CollectSettings());
            var simulationWindow = new View.SimulationWindow(kit);
            simulationWindow.Show();
            this.OnCloseWindow();
        }

        private Model.WorldSettings CollectSettings()
        {
            return new Model.WorldSettings()
                   {
                   StateReligionIndex = ((KeyValuePair<int, string>)this.SelectedReligion).Key,
                   ProvinceIndex = ((KeyValuePair<int, string>)this.SelectedProvince).Key,
                   FactionIndex = ((KeyValuePair<int, string>)this.SelectedFaction).Key,
                   FertilityDrop = (int)this.SelectedFertilityDrop,
                   WorstCaseWeather = (Model.ClimateAndWeather.Weather)Enum.Parse(typeof(Model.ClimateAndWeather.Weather), (string)this.selectedWeather),
                   DesiredTechnologyLevelIndex = (int)this.SelectedTechnologyTier,
                   UseLegacyTechnologies = this.UseLegacy
                   };
        }
    }
}