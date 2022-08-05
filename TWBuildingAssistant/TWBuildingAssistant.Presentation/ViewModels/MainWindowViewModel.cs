namespace TWBuildingAssistant.Presentation.ViewModels;

using System;
using TWBuildingAssistant.Model;

public class MainWindowViewModel 
    : WindowViewModel
{
    private readonly World world;

    private SettingsViewModel settingsViewModel;

    private ProvinceViewModel provinceViewModel;

    private SeekerViewModel seekerViewModel;

    private ViewModel content;

    public MainWindowViewModel()
    {
        this.world = new World();

        this.settingsViewModel = new SettingsViewModel(this.world);
        this.settingsViewModel.NextTransition += this.TransitionFromSettingsToProvince;
        this.content = this.settingsViewModel;
    }

    public SettingsViewModel Settings
    {
        get => this.settingsViewModel;
        set
        {
            if (this.settingsViewModel != value)
            {
                this.settingsViewModel = value;
                this.OnPropertyChanged(nameof(this.Settings));
            }
        }
    }

    public ProvinceViewModel Province
    {
        get => this.provinceViewModel;
        set
        {
            if (this.provinceViewModel != value)
            {
                this.provinceViewModel = value;
                this.OnPropertyChanged(nameof(this.Province));
            }
        }
    }

    public SeekerViewModel Seeker
    {
        get => this.seekerViewModel;
        set
        {
            if (this.seekerViewModel != value)
            {
                this.seekerViewModel = value;
                this.OnPropertyChanged(nameof(this.Seeker));
            }
        }
    }

    public ViewModel Content
    {
        get => this.content;
        set
        {
            if (this.content != value)
            {
                this.content = value;
                this.OnPropertyChanged(nameof(this.Content));
            }
        }
    }

    private void TransitionFromSettingsToProvince(object sender, SettingsViewModel.NextTransitionEventArgs args)
    {
        this.Province = new ProvinceViewModel(args.Province);
        this.Province.NextTransition += this.TransitionFromProvinceToSeeker;
        this.Province.PreviousTransition += this.TransitionFromProvinceToSettings;
        this.Content = this.provinceViewModel;
    }

    private void TransitionFromProvinceToSeeker(object sender, ProvinceViewModel.NextTransitionEventArgs args)
    {
        this.Seeker = new SeekerViewModel(args.Province, args.Slots);
        this.Seeker.PreviousTransition += this.TransitionFromSeekerToProvince;
        this.Content = this.Seeker;
    }

    private void TransitionFromProvinceToSettings(object sender, EventArgs args)
    {
        this.Content = this.Settings;
    }

    private void TransitionFromSeekerToProvince(object sender, SeekerViewModel.PreviousTransitionEventArgs args)
    {
        this.Province = new ProvinceViewModel(args.Province);
        this.Province.NextTransition += this.TransitionFromProvinceToSeeker;
        this.Province.PreviousTransition += this.TransitionFromProvinceToSettings;
        this.Content = this.Province;
    }
}