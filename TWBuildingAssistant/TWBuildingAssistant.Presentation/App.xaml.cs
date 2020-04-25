namespace TWBuildingAssistant.Presentation
{
    using Avalonia;
    using Avalonia.Controls.ApplicationLifetimes;
    using Avalonia.Markup.Xaml;
    using TWBuildingAssistant.Presentation.ViewModels;
    using TWBuildingAssistant.Presentation.Views;

    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new SettingsWindow();
                var viewModel = new SettingsWindowViewModel();
                desktop.MainWindow.DataContext = viewModel;
                viewModel.CloseWindow += (sender, e) => { desktop.MainWindow.Close(); };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
