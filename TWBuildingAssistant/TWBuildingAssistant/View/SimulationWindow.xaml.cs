namespace TWBuildingAssistant.View
{
    /// <summary>
    /// Interaction logic for SimulationWindow.xaml
    /// </summary>
    public partial class SimulationWindow
    {
        public SimulationWindow(Model.SimulationKit kit)
        {
            this.InitializeComponent();
            var viewModel = new ViewModel.ViewModelSimulationWindow(kit);
            this.DataContext = viewModel;
            viewModel.CloseWindow += (sender, e) => { this.Close(); };
        }
    }
}
