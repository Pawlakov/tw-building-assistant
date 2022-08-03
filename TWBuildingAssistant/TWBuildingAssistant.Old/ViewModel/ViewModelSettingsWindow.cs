namespace TWBuildingAssistant.Old.ViewModel;

using System.Collections.ObjectModel;
using TWBuildingAssistant.Model;

public class ViewModelSettingsWindow : ViewModelWindow
{
    private readonly World world;

    private object selectedReligion;

    private object selectedProvince;

    private object selectedFaction;

    public ViewModelSettingsWindow()
    {
        this.world = new World();
        this.Religions = new ObservableCollection<Religion>(this.world.Religions);
        this.SelectedReligion = this.Religions[0];
        this.Provinces = new ObservableCollection<Province>(this.world.Provinces);
        this.SelectedProvince = this.Provinces[0];
        this.Factions = new ObservableCollection<Faction>(this.world.Factions);
        this.SelectedFaction = this.Factions[0];
        this.SubmitCommand = new RelayCommand(this.Submit);
    }

    public ObservableCollection<Religion> Religions { get; set; }

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

    public ObservableCollection<Province> Provinces { get; set; }

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

    public ObservableCollection<Faction> Factions { get; set; }

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

    public RelayCommand SubmitCommand { get; }

    private void Submit(object paramter)
    {
        var religion = (Religion)this.SelectedReligion;
        var faction = (Faction)this.SelectedFaction;
        faction.StateReligion = religion;
        var province = (Province)this.SelectedProvince;
        province.Owner = faction;
        var simulationWindow = new View.SimulationWindow(province);
        simulationWindow.Show();
        this.OnCloseWindow();
    }
}