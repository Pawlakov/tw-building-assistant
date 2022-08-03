namespace TWBuildingAssistant.Presentation.ViewModels;

using ReactiveUI;
using System;
using TWBuildingAssistant.Model;

public class MainWindowViewModel : WindowViewModel
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
        set => this.RaiseAndSetIfChanged(ref this.settingsViewModel, value);
    }

    public ProvinceViewModel Province
    {
        get => this.provinceViewModel;
        set => this.RaiseAndSetIfChanged(ref this.provinceViewModel, value);
    }

    public SeekerViewModel Seeker
    {
        get => this.seekerViewModel;
        set => this.RaiseAndSetIfChanged(ref this.seekerViewModel, value);
    }

    public ViewModel Content
    {
        get => this.content;
        set => this.RaiseAndSetIfChanged(ref this.content, value);
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