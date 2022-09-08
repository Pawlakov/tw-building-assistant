namespace TWBuildingAssistant.Presentation.Views;

using System.Windows;

public partial class MainWindow 
    : Window
{
    public MainWindow(object dataContex)
    {
        this.InitializeComponent();
        this.DataContext = dataContex;
    }
}