namespace TWBuildingAssistant.Old.View;

/// <summary>
/// Interaction logic for SimulationWindow.xaml
/// </summary>
public partial class SimulationWindow
{
    public SimulationWindow(Model.Province province)
    {
        this.InitializeComponent();
        var viewModel = new ViewModel.ViewModelSimulationWindow(province);
        this.DataContext = viewModel;
        viewModel.CloseWindow += (sender, e) => { this.Close(); };
    }
}
