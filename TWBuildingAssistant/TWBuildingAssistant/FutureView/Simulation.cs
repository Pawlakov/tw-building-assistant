namespace TWBuildingAssistant.FutureView
{
    using System;
    using System.Windows;

    using TWBuildingAssistant.Model;

    public class Simulation : Application
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var simulation = new Simulation();
            simulation.Startup += SimulationStartup;
            simulation.Exit += SimulationExit;
            simulation.Run();
        }

        private static void SimulationStartup(object sender, StartupEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        private static void SimulationExit(object sender, ExitEventArgs e)
        {
            MessageBox.Show("Simulation ended.");
        }
    }
}