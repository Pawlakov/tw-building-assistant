namespace TWBuildingAssistant.Presentation.Views;

using System.Windows;
using TWBuildingAssistant.Presentation.ViewModels;

public partial class MainWindow 
    : Window
{
    public MainWindow(object dataContex)
    {
        this.InitializeComponent();
        this.DataContext = dataContex;
    }
}