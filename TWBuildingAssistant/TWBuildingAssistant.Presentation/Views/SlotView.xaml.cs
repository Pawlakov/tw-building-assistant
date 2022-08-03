namespace TWBuildingAssistant.Presentation.Views;

using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TWBuildingAssistant.Presentation.ViewModels;

public class SlotView : ReactiveUserControl<SlotViewModel>
{
    public SlotView()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.WhenActivated(disposables => { /* Handle view activation etc. */ });
        AvaloniaXamlLoader.Load(this);
    }
}
