using System;
using System.Windows;
using System.Windows.Controls;
namespace AdvancedSimulator
{
	// Główna klasa całego programu.
	public class Simulation : Application
	{
		// Świat symulacji.
		private static GameWorld.World _world;
		//
		[STAThread]
		public static void Main(string[] args)
		{
			Simulation simulation = new Simulation();
			simulation.Startup += SimulationStartup;
			simulation.Exit += SimulationExit;
			simulation.Run();
		}
		private static void SimulationStartup(object sender, StartupEventArgs e)
		{
			_world = new GameWorld.World();
			SettingsWindow settingsWindow = new SettingsWindow(_world.Religions, _world.Provinces, _world.Factions);
			settingsWindow.SubmittingSettings += (GameWorld.WorldSettings settings) =>
			{
				GameWorld.SimulationKit kit = _world.AssembleSimulationKit(settings);
				SimulationWindow simulationWindow = new SimulationWindow(kit);
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