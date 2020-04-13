namespace TWBuildingAssistant.WorldManager.Views.Resources
{
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

    public class AddView : UserControl
    {
        public AddView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
