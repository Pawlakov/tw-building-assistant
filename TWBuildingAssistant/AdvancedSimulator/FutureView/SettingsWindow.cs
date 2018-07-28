namespace TWBuildingAssistant.FutureView
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using TWBuildingAssistant.Model;

    public class SettingsWindow : Window
    {
        private readonly Canvas _canvas = new Canvas() { Width = 400, Height = 225 };

        private readonly Button _submitButton = new Button() { Width = 100, Height = 25, Content = "Submit settings" };

        private readonly ComboBox _religionsDropdown = new ComboBox() { Width = 100, Height = 25 };

        private readonly ComboBox _provincesDropdown = new ComboBox() { Width = 100, Height = 25 };

        private readonly ComboBox _factionsDropdown = new ComboBox() { Width = 100, Height = 25 };

        private readonly Label _religionsLabel = new Label() { Width = 100, Height = 25, Content = "State religion:", HorizontalContentAlignment = HorizontalAlignment.Left };
        
        private readonly Label _provincesLabel = new Label() { Width = 100, Height = 25, Content = "Province:", HorizontalContentAlignment = HorizontalAlignment.Left };

        private readonly Label _factionsLabel = new Label() { Width = 100, Height = 25, Content = "Faction:", HorizontalContentAlignment = HorizontalAlignment.Left };

        private readonly ComboBox _fertilityDropDropdown = new ComboBox() { Width = 100, Height = 25 };
        
        private readonly ComboBox _worstCaseWeatherDropdown = new ComboBox() { Width = 100, Height = 25 };
        
        private readonly Label _fertilityDropLabel = new Label() { Width = 100, Height = 25, Content = "Fertility drop:", HorizontalContentAlignment = HorizontalAlignment.Left };
        
        private readonly Label _worstCaseWeatherLabel = new Label() { Width = 100, Height = 25, Content = "Worst case weather::", HorizontalContentAlignment = HorizontalAlignment.Left };

        private readonly ComboBox _technologyTiersDropdown = new ComboBox() { Width = 100, Height = 25 };
        
        private readonly CheckBox _useLegacyTechnologiesCheckbox = new CheckBox();
        
        private readonly Label _technologyTiersLabel = new Label() { Width = 100, Height = 25, Content = "Technology tier:", HorizontalContentAlignment = HorizontalAlignment.Left };

        private readonly Label _useLegacyTechnologiesLabel = new Label() { Width = 100, Height = 25, Content = "Use legacy techs:", HorizontalContentAlignment = HorizontalAlignment.Left };

        private void SubmitButtonClicked(object sender, RoutedEventArgs e)
        {
            this.SubmittingSettings?.Invoke(this.Settings);
            this.Close();
        }

        public event SettingsSubmittingHandler SubmittingSettings;

        public SettingsWindow(IEnumerable<KeyValuePair<int, string>> religions, IEnumerable<KeyValuePair<int, string>> provinces, IEnumerable<KeyValuePair<int, string>> factions)
        {
            Canvas.SetTop(this._religionsLabel, 0);
            Canvas.SetLeft(this._religionsLabel, 25);
            //
            this._religionsDropdown.ItemsSource = religions;
            this._religionsDropdown.SelectedIndex = 0;
            Canvas.SetTop(this._religionsDropdown, 25);
            Canvas.SetLeft(this._religionsDropdown, 25);
            //
            Canvas.SetTop(this._provincesLabel, 50);
            Canvas.SetLeft(this._provincesLabel, 25);
            //
            this._provincesDropdown.ItemsSource = provinces;
            this._provincesDropdown.SelectedIndex = 0;
            Canvas.SetTop(this._provincesDropdown, 75);
            Canvas.SetLeft(this._provincesDropdown, 25);
            //
            Canvas.SetTop(this._factionsLabel, 100);
            Canvas.SetLeft(this._factionsLabel, 25);
            //
            this._factionsDropdown.ItemsSource = factions;
            this._factionsDropdown.SelectedIndex = 0;
            Canvas.SetTop(this._factionsDropdown, 125);
            Canvas.SetLeft(this._factionsDropdown, 25);
            //
            Canvas.SetTop(this._fertilityDropLabel, 50);
            Canvas.SetLeft(this._fertilityDropLabel, 150);
            //
            this._fertilityDropDropdown.ItemsSource = new List<int>() { 0, 1, 2, 3, 4 };
            this._fertilityDropDropdown.SelectedIndex = 0;
            Canvas.SetTop(this._fertilityDropDropdown, 75);
            Canvas.SetLeft(this._fertilityDropDropdown, 150);
            //
            Canvas.SetTop(this._worstCaseWeatherLabel, 100);
            Canvas.SetLeft(this._worstCaseWeatherLabel, 150);
            //
            this._worstCaseWeatherDropdown.ItemsSource = new List<string>() { "Good", "Normal", "Bad", "Extreme" };
            this._worstCaseWeatherDropdown.SelectedIndex = 0;
            Canvas.SetTop(this._worstCaseWeatherDropdown, 125);
            Canvas.SetLeft(this._worstCaseWeatherDropdown, 150);
            //
            Canvas.SetTop(this._technologyTiersLabel, 0);
            Canvas.SetLeft(this._technologyTiersLabel, 275);
            //
            this._technologyTiersDropdown.ItemsSource = new List<int>() { 0, 1, 2, 3, 4 };
            this._technologyTiersDropdown.SelectedIndex = 0;
            Canvas.SetTop(this._technologyTiersDropdown, 25);
            Canvas.SetLeft(this._technologyTiersDropdown, 275);
            //
            Canvas.SetTop(this._useLegacyTechnologiesLabel, 50);
            Canvas.SetLeft(this._useLegacyTechnologiesLabel, 275);
            //
            Canvas.SetTop(this._useLegacyTechnologiesCheckbox, 75);
            Canvas.SetLeft(this._useLegacyTechnologiesCheckbox, 275);
            //
            this._submitButton.Click += this.SubmitButtonClicked;
            Canvas.SetTop(this._submitButton, 175);
            Canvas.SetLeft(this._submitButton, 150);
            //
            this._canvas.Children.Add(this._religionsLabel);
            this._canvas.Children.Add(this._religionsDropdown);
            this._canvas.Children.Add(this._provincesLabel);
            this._canvas.Children.Add(this._provincesDropdown);
            this._canvas.Children.Add(this._factionsLabel);
            this._canvas.Children.Add(this._factionsDropdown);
            //
            this._canvas.Children.Add(this._fertilityDropLabel);
            this._canvas.Children.Add(this._fertilityDropDropdown);
            this._canvas.Children.Add(this._worstCaseWeatherLabel);
            this._canvas.Children.Add(this._worstCaseWeatherDropdown);
            //
            this._canvas.Children.Add(this._technologyTiersLabel);
            this._canvas.Children.Add(this._technologyTiersDropdown);
            this._canvas.Children.Add(this._useLegacyTechnologiesLabel);
            this._canvas.Children.Add(this._useLegacyTechnologiesCheckbox);
            //
            this._canvas.Children.Add(this._submitButton);
            //
            this.ResizeMode = ResizeMode.CanMinimize;
            this.Title = "Simulation settings";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.Content = this._canvas;
        }
        
        public WorldSettings Settings
        {
            get
            {
                return new WorldSettings()
                {
                    StateReligionIndex = ((KeyValuePair<int, string>)this._religionsDropdown.SelectedItem).Key,
                    ProvinceIndex = ((KeyValuePair<int, string>)this._provincesDropdown.SelectedItem).Key,
                    FactionIndex = ((KeyValuePair<int, string>)this._factionsDropdown.SelectedItem).Key,
                    FertilityDrop = (int)this._fertilityDropDropdown.SelectedItem,
                    WorstCaseWeather = (Model.ClimateAndWeather.Weather)Enum.Parse(typeof(Model.ClimateAndWeather.Weather), (string)this._worstCaseWeatherDropdown.SelectedItem),
                    DesiredTechnologyLevelIndex = (int)this._technologyTiersDropdown.SelectedItem,
                    UseLegacyTechnologies = this._useLegacyTechnologiesCheckbox.IsChecked.Value
                };
            }
        }
    }

    public delegate void SettingsSubmittingHandler(WorldSettings settings);
}