namespace TWBuildingAssistant.WorldManager.Views.Resources
{
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

    public class ListView : UserControl
    {
        public ListView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}