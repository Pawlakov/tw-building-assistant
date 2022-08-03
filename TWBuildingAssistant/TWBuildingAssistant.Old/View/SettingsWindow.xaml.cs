namespace TWBuildingAssistant.Old.View;

public partial class SettingsWindow
{
    public SettingsWindow()
    {
        this.InitializeComponent();
        var viewModel = new ViewModel.ViewModelSettingsWindow();
        this.DataContext = viewModel;
        viewModel.CloseWindow += (sender, e) => { this.Close(); };
    }
}
