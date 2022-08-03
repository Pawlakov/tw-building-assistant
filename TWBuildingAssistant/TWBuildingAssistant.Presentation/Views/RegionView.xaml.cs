namespace TWBuildingAssistant.Presentation.Views;

using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TWBuildingAssistant.Presentation.ViewModels;

public class RegionView : ReactiveUserControl<RegionViewModel>
{
    public RegionView()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.WhenActivated(disposables => { /* Handle view activation etc. */ });
        AvaloniaXamlLoader.Load(this);
    }
}
