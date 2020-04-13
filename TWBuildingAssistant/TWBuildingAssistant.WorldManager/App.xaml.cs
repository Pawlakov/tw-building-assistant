namespace TWBuildingAssistant.WorldManager
{
    using Avalonia;
    using Avalonia.Controls.ApplicationLifetimes;
    using Avalonia.Markup.Xaml;
    using TWBuildingAssistant.Data.Sqlite;
    using TWBuildingAssistant.WorldManager.ViewModels;
    using TWBuildingAssistant.WorldManager.Views;

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
                var context = new DatabaseContext();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(context),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
