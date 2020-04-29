﻿namespace TWBuildingAssistant.Presentation.ViewModels
{
    using System.Collections.ObjectModel;
    using ReactiveUI;
    using TWBuildingAssistant.Model;

    public class MainWindowViewModel : WindowViewModel
    {
        private readonly World world;

        private Religion selectedReligion;

        private Province selectedProvince;

        private Faction selectedFaction;

        private int selectedTechnologyTier;

        private bool useAntilegacyTechnologies;

        private ProvinceViewModel provinceViewModel;

        public MainWindowViewModel()
        {
            this.world = new World();
            this.Religions = new ObservableCollection<Religion>(this.world.Religions);
            this.selectedReligion = this.Religions[0];
            this.Provinces = new ObservableCollection<Province>(this.world.Provinces);
            this.selectedProvince = this.Provinces[0];
            this.Factions = new ObservableCollection<Faction>(this.world.Factions);
            this.selectedFaction = this.Factions[0];
            this.TechnologyTiers = new ObservableCollection<int>(new int[] { 1, 2, 3, 4 });
            this.selectedTechnologyTier = this.TechnologyTiers[0];
            this.useAntilegacyTechnologies = false;

            this.UpdateProvince();
        }

        public ObservableCollection<Religion> Religions { get; set; }

        public Religion SelectedReligion
        {
            get => this.selectedReligion;
            set
            {
                this.RaiseAndSetIfChanged(ref this.selectedReligion, value);
                this.UpdateProvince();
            }
        }

        public ObservableCollection<Province> Provinces { get; set; }

        public Province SelectedProvince
        {
            get => this.selectedProvince;
            set
            {
                this.RaiseAndSetIfChanged(ref this.selectedProvince, value);
                this.UpdateProvince();
            }
        }

        public ObservableCollection<Faction> Factions { get; set; }

        public Faction SelectedFaction
        {
            get => this.selectedFaction;
            set
            {
                this.RaiseAndSetIfChanged(ref this.selectedFaction, value);
                this.UpdateProvince();
            }
        }

        public ObservableCollection<int> TechnologyTiers { get; set; }

        public int SelectedTechnologyTier
        {
            get => this.selectedTechnologyTier;
            set
            {
                this.RaiseAndSetIfChanged(ref this.selectedTechnologyTier, value);
                this.UpdateProvince();
            }
        }

        public bool UseAntilegacyTechnologies
        {
            get => this.useAntilegacyTechnologies;
            set
            {
                this.RaiseAndSetIfChanged(ref this.useAntilegacyTechnologies, value);
                this.UpdateProvince();
            }
        }

        public ProvinceViewModel Province
        {
            get => this.provinceViewModel;
            set => this.RaiseAndSetIfChanged(ref this.provinceViewModel, value);
        }

        private void UpdateProvince()
        {
            this.selectedFaction.TechnologyTier = this.selectedTechnologyTier;
            this.selectedFaction.UseAntilegacyTechnologies = this.useAntilegacyTechnologies;
            this.selectedFaction.StateReligion = this.selectedReligion;
            this.selectedProvince.Owner = this.selectedFaction;
            var viewModel = new ProvinceViewModel(this.selectedProvince);
            this.Province = viewModel;
        }
    }
}