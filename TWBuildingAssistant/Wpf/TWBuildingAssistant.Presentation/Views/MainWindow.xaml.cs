namespace TWBuildingAssistant.Presentation.Views;

using System.Windows;

public partial class MainWindow 
    : Window
{
    public MainWindow(object dataContext)
    {
        this.InitializeComponent();
        this.DataContext = dataContext;
    }
}