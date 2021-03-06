﻿namespace TWBuildingAssistant.Presentation.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using ReactiveUI;
    using TWBuildingAssistant.Model;

    public class SettingsViewModel : ViewModel
    {
        private readonly World world;

        private Religion selectedReligion;

        private Province selectedProvince;

        private Faction selectedFaction;

        private int selectedTechnologyTier;

        private bool useAntilegacyTechnologies;

        private int selectedFertilityDrop;

        private Weather selectedWeather;

        private Season selectedSeason;

        private int corruptionRate;

        public SettingsViewModel(World world)
        {
            this.world = world;
            this.Religions = new ObservableCollection<Religion>(this.world.Religions);
            this.selectedReligion = this.Religions[0];
            this.Provinces = new ObservableCollection<Province>(this.world.Provinces);
            this.selectedProvince = this.Provinces[0];
            this.Factions = new ObservableCollection<Faction>(this.world.Factions);
            this.selectedFaction = this.Factions[0];
            this.TechnologyTiers = new ObservableCollection<int>(new int[] { 0, 1, 2, 3, 4 });
            this.selectedTechnologyTier = this.TechnologyTiers[0];
            this.useAntilegacyTechnologies = false;
            this.FertilityDrops = new ObservableCollection<int>(new int[] { 0, -1, -2, -3, -4 });
            this.selectedFertilityDrop = this.FertilityDrops[0];
            this.Weathers = new ObservableCollection<Weather>(this.world.Weathers);
            this.selectedWeather = this.Weathers[0];
            this.Seasons = new ObservableCollection<Season>(this.world.Seasons);
            this.selectedSeason = this.Seasons[0];
            this.corruptionRate = 1;
        }

        public event EventHandler<NextTransitionEventArgs> NextTransition;

        public ObservableCollection<Religion> Religions { get; set; }

        public Religion SelectedReligion
        {
            get => this.selectedReligion;
            set
            {
                this.RaiseAndSetIfChanged(ref this.selectedReligion, value);
            }
        }

        public ObservableCollection<Province> Provinces { get; set; }

        public Province SelectedProvince
        {
            get => this.selectedProvince;
            set
            {
                this.RaiseAndSetIfChanged(ref this.selectedProvince, value);
            }
        }

        public ObservableCollection<Faction> Factions { get; set; }

        public Faction SelectedFaction
        {
            get => this.selectedFaction;
            set
            {
                this.RaiseAndSetIfChanged(ref this.selectedFaction, value);
            }
        }

        public ObservableCollection<int> TechnologyTiers { get; set; }

        public int SelectedTechnologyTier
        {
            get => this.selectedTechnologyTier;
            set
            {
                this.RaiseAndSetIfChanged(ref this.selectedTechnologyTier, value);
            }
        }

        public bool UseAntilegacyTechnologies
        {
            get => this.useAntilegacyTechnologies;
            set
            {
                this.RaiseAndSetIfChanged(ref this.useAntilegacyTechnologies, value);
            }
        }

        public ObservableCollection<int> FertilityDrops { get; set; }

        public int SelectedFertilityDrop
        {
            get => this.selectedFertilityDrop;
            set
            {
                this.RaiseAndSetIfChanged(ref this.selectedFertilityDrop, value);
            }
        }

        public ObservableCollection<Weather> Weathers { get; set; }

        public Weather SelectedWeather
        {
            get => this.selectedWeather;
            set
            {
                this.RaiseAndSetIfChanged(ref this.selectedWeather, value);
            }
        }

        public ObservableCollection<Season> Seasons { get; set; }

        public Season SelectedSeason
        {
            get => this.selectedSeason;
            set
            {
                this.RaiseAndSetIfChanged(ref this.selectedSeason, value);
            }
        }

        public int CorruptionRate
        {
            get => this.corruptionRate;
            set
            {
                if (value > 99)
                {
                    value = 99;
                }
                else if (value < 1)
                {
                    value = 1;
                }

                this.RaiseAndSetIfChanged(ref this.corruptionRate, value);
            }
        }

        public void Next()
        {
            this.SelectedFaction.FertilityDrop = this.SelectedFertilityDrop;
            this.SelectedFaction.TechnologyTier = this.SelectedTechnologyTier;
            this.SelectedFaction.UseAntilegacyTechnologies = this.UseAntilegacyTechnologies;
            this.SelectedFaction.StateReligion = this.SelectedReligion;
            this.SelectedProvince.Owner = this.SelectedFaction;
            this.SelectedProvince.Weather = this.SelectedWeather;
            this.SelectedProvince.Season = this.SelectedSeason;
            this.SelectedProvince.CorruptionRate = this.CorruptionRate;

            this.NextTransition?.Invoke(this, new NextTransitionEventArgs(this.selectedProvince));
        }

        public class NextTransitionEventArgs : EventArgs
        {
            public NextTransitionEventArgs(Province province)
            {
                this.Province = province;
            }

            public Province Province { get; }
        }
    }
}