using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
namespace AdvancedSimulator
{
	// Okno służące do wybierania ustawień świata symulacji.
	public class SettingsWindow : Window
	{
		private readonly Canvas _canvas = new Canvas() { Width = 400, Height = 225 };
		private readonly Button _submitButton = new Button() { Width = 100, Height = 25, Content = "Submit settings" };
		//
		private readonly ComboBox _religionsDropdown = new ComboBox() { Width = 100, Height = 25 };
		private readonly ComboBox _provincesDropdown = new ComboBox() { Width = 100, Height = 25 };
		private readonly ComboBox _factionsDropdown = new ComboBox() { Width = 100, Height = 25 };
		private readonly Label _religionsLabel = new Label() { Width = 100, Height = 25, Content = "State religion:", HorizontalContentAlignment = HorizontalAlignment.Left };
		private readonly Label _provincesLabel = new Label() { Width = 100, Height = 25, Content = "Province:", HorizontalContentAlignment = HorizontalAlignment.Left };
		private readonly Label _factionsLabel = new Label() { Width = 100, Height = 25, Content = "Faction:", HorizontalContentAlignment = HorizontalAlignment.Left };
		//
		private readonly ComboBox _fertilityDropDropdown = new ComboBox() { Width = 100, Height = 25 };
		private readonly ComboBox _worstCaseWeatherDropdown = new ComboBox() { Width = 100, Height = 25 };
		private readonly Label _fertilityDropLabel = new Label() { Width = 100, Height = 25, Content = "Fertility drop:", HorizontalContentAlignment = HorizontalAlignment.Left };
		private readonly Label _worstCaseWeatherLabel = new Label() { Width = 100, Height = 25, Content = "Worst case weather::", HorizontalContentAlignment = HorizontalAlignment.Left };
		//
		private readonly ComboBox _technologyTiersDropdown = new ComboBox() { Width = 100, Height = 25 };
		private readonly CheckBox _useLegacyTechnologiesCheckbox = new CheckBox();
		private readonly Label _technologyTiersLabel = new Label() { Width = 100, Height = 25, Content = "Technology tier:", HorizontalContentAlignment = HorizontalAlignment.Left };
		private readonly Label _useLegacyTechnologiesLabel = new Label() { Width = 100, Height = 25, Content = "Use legacy techs:", HorizontalContentAlignment = HorizontalAlignment.Left };
		//
		private void SubmitButtonClicked(object sender, RoutedEventArgs e)
		{
			SubmittingSettings?.Invoke(Settings);
			Close();
		}
		public event SettingsSubmittingHandler SubmittingSettings;
		//
		public SettingsWindow(IEnumerable<KeyValuePair<int, string>> religions, IEnumerable<KeyValuePair<int, string>> provinces, IEnumerable<KeyValuePair<int, string>> factions)
		{
			Canvas.SetTop(_religionsLabel, 0);
			Canvas.SetLeft(_religionsLabel, 25);
			//
			_religionsDropdown.ItemsSource = religions;
			_religionsDropdown.SelectedIndex = 0;
			Canvas.SetTop(_religionsDropdown, 25);
			Canvas.SetLeft(_religionsDropdown, 25);
			//
			Canvas.SetTop(_provincesLabel, 50);
			Canvas.SetLeft(_provincesLabel, 25);
			//
			_provincesDropdown.ItemsSource = provinces;
			_provincesDropdown.SelectedIndex = 0;
			Canvas.SetTop(_provincesDropdown, 75);
			Canvas.SetLeft(_provincesDropdown, 25);
			//
			Canvas.SetTop(_factionsLabel, 100);
			Canvas.SetLeft(_factionsLabel, 25);
			//
			_factionsDropdown.ItemsSource = factions;
			_factionsDropdown.SelectedIndex = 0;
			Canvas.SetTop(_factionsDropdown, 125);
			Canvas.SetLeft(_factionsDropdown, 25);
			//
			Canvas.SetTop(_fertilityDropLabel, 50);
			Canvas.SetLeft(_fertilityDropLabel, 150);
			//
			_fertilityDropDropdown.ItemsSource = new List<int>() { 0, 1, 2, 3, 4 };
			_fertilityDropDropdown.SelectedIndex = 0;
			Canvas.SetTop(_fertilityDropDropdown, 75);
			Canvas.SetLeft(_fertilityDropDropdown, 150);
			//
			Canvas.SetTop(_worstCaseWeatherLabel, 100);
			Canvas.SetLeft(_worstCaseWeatherLabel, 150);
			//
			_worstCaseWeatherDropdown.ItemsSource = new List<string>() { "Good", "Normal", "Bad", "Extreme" };
			_worstCaseWeatherDropdown.SelectedIndex = 0;
			Canvas.SetTop(_worstCaseWeatherDropdown, 125);
			Canvas.SetLeft(_worstCaseWeatherDropdown, 150);
			//
			Canvas.SetTop(_technologyTiersLabel, 0);
			Canvas.SetLeft(_technologyTiersLabel, 275);
			//
			_technologyTiersDropdown.ItemsSource = new List<int>() { 0, 1, 2, 3, 4 };
			_technologyTiersDropdown.SelectedIndex = 0;
			Canvas.SetTop(_technologyTiersDropdown, 25);
			Canvas.SetLeft(_technologyTiersDropdown, 275);
			//
			Canvas.SetTop(_useLegacyTechnologiesLabel, 50);
			Canvas.SetLeft(_useLegacyTechnologiesLabel, 275);
			//
			Canvas.SetTop(_useLegacyTechnologiesCheckbox, 75);
			Canvas.SetLeft(_useLegacyTechnologiesCheckbox, 275);
			//
			_submitButton.Click += SubmitButtonClicked;
			Canvas.SetTop(_submitButton, 175);
			Canvas.SetLeft(_submitButton, 150);
			//
			_canvas.Children.Add(_religionsLabel);
			_canvas.Children.Add(_religionsDropdown);
			_canvas.Children.Add(_provincesLabel);
			_canvas.Children.Add(_provincesDropdown);
			_canvas.Children.Add(_factionsLabel);
			_canvas.Children.Add(_factionsDropdown);
			//
			_canvas.Children.Add(_fertilityDropLabel);
			_canvas.Children.Add(_fertilityDropDropdown);
			_canvas.Children.Add(_worstCaseWeatherLabel);
			_canvas.Children.Add(_worstCaseWeatherDropdown);
			//
			_canvas.Children.Add(_technologyTiersLabel);
			_canvas.Children.Add(_technologyTiersDropdown);
			_canvas.Children.Add(_useLegacyTechnologiesLabel);
			_canvas.Children.Add(_useLegacyTechnologiesCheckbox);
			//
			_canvas.Children.Add(_submitButton);
			//
			ResizeMode = ResizeMode.CanMinimize;
			Title = "Simulation settings";
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
			SizeToContent = SizeToContent.WidthAndHeight;
			Content = _canvas;
		}
		public GameWorld.WorldSettings Settings
		{
			get
			{
				return new GameWorld.WorldSettings()
				{
					StateReligionIndex = ((KeyValuePair<int, string>)_religionsDropdown.SelectedItem).Key,
					ProvinceIndex = ((KeyValuePair<int, string>)_provincesDropdown.SelectedItem).Key,
					FactionIndex = ((KeyValuePair<int, string>)_factionsDropdown.SelectedItem).Key,
					FertilityDrop = (int)_fertilityDropDropdown.SelectedItem,
					WorstCaseWeather = (GameWorld.ClimateAndWeather.Weather)Enum.Parse(typeof(GameWorld.ClimateAndWeather.Weather), (string)_worstCaseWeatherDropdown.SelectedItem),
					DesiredTechnologyLevelIndex = (int)_technologyTiersDropdown.SelectedItem,
					UseLegacyTechnologies = _useLegacyTechnologiesCheckbox.IsChecked.Value
				};
			}
		}
	}
	public delegate void SettingsSubmittingHandler(GameWorld.WorldSettings settings);
}
