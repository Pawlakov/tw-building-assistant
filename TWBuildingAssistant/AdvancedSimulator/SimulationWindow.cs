using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;
namespace AdvancedSimulator
{
	// Okno do przeprowadzania właściwej symulacji.
	public class SimulationWindow : Window
	{
		private readonly GameWorld.SimulationKit _simulation;
		private GameWorld.SimulationKit.CombinationPerformance _currentPerformance;
		//
		private readonly StackPanel _mainPanel = new StackPanel() { Orientation = Orientation.Vertical };
		private readonly StackPanel _combinationPanel = new StackPanel() { Orientation = Orientation.Horizontal };
		private readonly StackPanel[] _regionsPanels;
		private readonly ComboBox[][] _slotsDropdowns;
		//
		private readonly Label[] _performanceLabeles = new Label[8]
		{
			new Label(),
			new Label(),
			new Label(),
			new Label(),
			new Label(),
			new Label(),
			new Label(),
			new Label()
		};
		//
		public SimulationWindow(GameWorld.SimulationKit kit)
		{
			_simulation = kit;
			// Kombinacja.
			_mainPanel.Children.Add(new Label() { Content = _simulation.ProvinceName(), HorizontalAlignment = HorizontalAlignment.Center });
			_mainPanel.Children.Add(_combinationPanel);
			_regionsPanels = new StackPanel[_simulation.GetRegionsCount()];
			_slotsDropdowns = new ComboBox[_regionsPanels.Length][];
			for (int whichRegion = 0; whichRegion < _slotsDropdowns.Length; ++whichRegion)
			{
				_regionsPanels[whichRegion] = new StackPanel() { Orientation = Orientation.Vertical };
				_combinationPanel.Children.Add(_regionsPanels[whichRegion]);
				_regionsPanels[whichRegion].Children.Add(new Label() { Content = _simulation.RegionName(whichRegion), HorizontalAlignment = HorizontalAlignment.Center });
				_slotsDropdowns[whichRegion] = new ComboBox[_simulation.GetSlotsCount(whichRegion)];
				for (int whichSlot = 0; whichSlot < _slotsDropdowns[whichRegion].Length; ++whichSlot)
				{
					_slotsDropdowns[whichRegion][whichSlot] = new ComboBox
					{
						ItemsSource = _simulation.GetAvailableBuildingsAt(whichRegion, whichSlot),
						SelectedIndex = 0
					};
					_simulation.SetBuildingAt(whichRegion, whichSlot, 0);
					_slotsDropdowns[whichRegion][whichSlot].SelectionChanged += HandleChange;
					_regionsPanels[whichRegion].Children.Add(_slotsDropdowns[whichRegion][whichSlot]);
				}
			}
			// Wyniki kombinacji.
			_currentPerformance = _simulation.CurrentPerformance();
			foreach (Label label in _performanceLabeles)
				_mainPanel.Children.Add(label);
			SetPerformanceDisplay();
			// Ustawienia okna.
			ResizeMode = ResizeMode.CanMinimize;
			Title = "Simulation";
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
			SizeToContent = SizeToContent.WidthAndHeight;
			Content = _mainPanel;
		}
		// W przypadku jakieś zmainy w kombinacji.
		private void HandleChange(object sender, SelectionChangedEventArgs e)
		{
			for (int whichRegion = 0; whichRegion < _slotsDropdowns.Length; ++whichRegion)
			{
				for (int whichSlot = 0; whichSlot < _slotsDropdowns[whichRegion].Length; ++whichSlot)
				{
					// Sprawdzenie który slot się zmienił.
					if (sender == _slotsDropdowns[whichRegion][whichSlot])
					{
						_simulation.SetBuildingAt(whichRegion, whichSlot, ((KeyValuePair<int, string>)_slotsDropdowns[whichRegion][whichSlot].SelectedItem).Key);
						_currentPerformance = _simulation.CurrentPerformance();
						SetPerformanceDisplay();
						return;
					}
				}
			}
		}
		// Zmiana wyświetlanych wartości na aktualne wyniki.
		private void SetPerformanceDisplay()
		{
			// Sładanie reprezentacji wartości higieny.
			string sanitationString = _currentPerformance.Sanitation[0].ToString();
			for (int whichRegion = 1; whichRegion < _currentPerformance.Sanitation.Length; whichRegion++)
				sanitationString += ("/" + _currentPerformance.Sanitation[whichRegion].ToString());
			//
			_performanceLabeles[0].Content = "Sanitation: " + sanitationString;
			_performanceLabeles[1].Content = "Food: " + _currentPerformance.Food.ToString();
			_performanceLabeles[2].Content = "Public Order: " + _currentPerformance.PublicOrder.ToString();
			_performanceLabeles[3].Content = "Relgious Osmosis: " + _currentPerformance.ReligiousOsmosis.ToString();
			_performanceLabeles[4].Content = "Fertility: " + _currentPerformance.Fertility.ToString();
			_performanceLabeles[5].Content = "Research Rate: +" + _currentPerformance.ResearchRate.ToString() + "%";
			_performanceLabeles[6].Content = "Growth: " + _currentPerformance.Growth.ToString();
			_performanceLabeles[7].Content = "Wealth: " + _currentPerformance.Wealth.ToString();
			// Zmiana kolory w zależności czy konflikt wystąpił czy nie.
			if (_currentPerformance.Conflict)
			{
				foreach (Label label in _performanceLabeles)
					label.Foreground = Brushes.Red;
			}
			else
			{
				foreach (Label label in _performanceLabeles)
					label.Foreground = Brushes.Black;
			}
		}
	}
}
