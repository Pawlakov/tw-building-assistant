namespace TWBuildingAssistant.FutureView
{
    using System;
    using System.Windows;

    using TWBuildingAssistant.Model;

    public class Simulation : Application
    {
        private static World world;

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
            world = new World();
            var settingsWindow = new SettingsWindow(world.Religions, world.Provinces, world.Factions);
            settingsWindow.SubmittingSettings += (settings) =>
                {
                    var kit = world.AssembleSimulationKit(settings);
                    var simulationWindow = new SimulationWindow(kit);
                    simulationWindow.Show();
                };
            settingsWindow.Show();
        }

        private static void SimulationExit(object sender, ExitEventArgs e)
        {
            MessageBox.Show("Simulation ended.");
        }
    }
}