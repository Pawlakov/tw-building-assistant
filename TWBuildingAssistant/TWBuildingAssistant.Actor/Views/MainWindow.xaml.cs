namespace TWBuildingAssistant.Actor.Views;

using System;
using System.Windows;
using System.Windows.Media.Imaging;

public partial class MainWindow
    : Window
{
    public MainWindow(object dataContext)
    {
        this.InitializeComponent();
        this.DataContext = dataContext;
    }
}