namespace TWBuildingAssistant.FutureView
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    // Okno do przeprowadzania właściwej symulacji.
    public class SimulationWindow : Window
    {
        private readonly Model.SimulationKit simulation;

        private Model.SimulationKit.CombinationPerformance currentPerformance;

        private readonly StackPanel mainPanel = new StackPanel() { Orientation = Orientation.Vertical };

        private readonly StackPanel combinationPanel = new StackPanel() { Orientation = Orientation.Horizontal };

        private readonly StackPanel[] regionsPanels;

        private readonly ComboBox[][] slotsDropdowns;

        private readonly Label[] performanceLabeles = new Label[8]
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

        public SimulationWindow(Model.SimulationKit kit)
        {
            this.simulation = kit;
            this.mainPanel.Children.Add(new Label() { Content = this.simulation.ProvinceName(), HorizontalAlignment = HorizontalAlignment.Center });
            this.mainPanel.Children.Add(this.combinationPanel);
            this.regionsPanels = new StackPanel[this.simulation.GetRegionsCount()];
            this.slotsDropdowns = new ComboBox[this.regionsPanels.Length][];
            for (var whichRegion = 0; whichRegion < this.slotsDropdowns.Length; ++whichRegion)
            {
                this.regionsPanels[whichRegion] = new StackPanel() { Orientation = Orientation.Vertical };
                this.combinationPanel.Children.Add(this.regionsPanels[whichRegion]);
                this.regionsPanels[whichRegion].Children.Add(new Label() { Content = this.simulation.RegionName(whichRegion), HorizontalAlignment = HorizontalAlignment.Center });
                this.slotsDropdowns[whichRegion] = new ComboBox[this.simulation.GetSlotsCount(whichRegion)];
                for (var whichSlot = 0; whichSlot < this.slotsDropdowns[whichRegion].Length; ++whichSlot)
                {
                    this.slotsDropdowns[whichRegion][whichSlot] = new ComboBox
                                                                  {
                                                                  ItemsSource = this.simulation.GetAvailableBuildingsAt(whichRegion, whichSlot),
                                                                  SelectedIndex = 0
                                                                  };
                    this.simulation.SetBuildingAt(whichRegion, whichSlot, 0);
                    this.slotsDropdowns[whichRegion][whichSlot].SelectionChanged += this.HandleChange;
                    this.regionsPanels[whichRegion].Children.Add(this.slotsDropdowns[whichRegion][whichSlot]);
                }
            }

            this.currentPerformance = this.simulation.CurrentPerformance();
            foreach (var label in this.performanceLabeles)
            {
                this.mainPanel.Children.Add(label);
            }

            this.SetPerformanceDisplay();
            this.ResizeMode = ResizeMode.CanMinimize;
            this.Title = "Simulation";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.Content = this.mainPanel;
        }

        private void HandleChange(object sender, SelectionChangedEventArgs e)
        {
            for (int whichRegion = 0; whichRegion < this.slotsDropdowns.Length; ++whichRegion)
            {
                for (int whichSlot = 0; whichSlot < this.slotsDropdowns[whichRegion].Length; ++whichSlot)
                {
                    // Sprawdzenie który slot się zmienił.
                    if (sender == this.slotsDropdowns[whichRegion][whichSlot])
                    {
                        this.simulation.SetBuildingAt(whichRegion, whichSlot, ((KeyValuePair<int, string>)this.slotsDropdowns[whichRegion][whichSlot].SelectedItem).Key);
                        this.currentPerformance = this.simulation.CurrentPerformance();
                        this.SetPerformanceDisplay();
                        return;
                    }
                }
            }
        }

        private void SetPerformanceDisplay()
        {
            // Sładanie reprezentacji wartości higieny.
            string sanitationString = this.currentPerformance.Sanitation[0].ToString();
            for (int whichRegion = 1; whichRegion < this.currentPerformance.Sanitation.Length; whichRegion++)
                sanitationString += ("/" + this.currentPerformance.Sanitation[whichRegion].ToString());
            //
            this.performanceLabeles[0].Content = "Sanitation: " + sanitationString;
            this.performanceLabeles[1].Content = "Food: " + this.currentPerformance.Food.ToString();
            this.performanceLabeles[2].Content = "Public Order: " + this.currentPerformance.PublicOrder.ToString();
            this.performanceLabeles[3].Content = "Relgious Osmosis: " + this.currentPerformance.ReligiousOsmosis.ToString();
            this.performanceLabeles[4].Content = "Fertility: " + this.currentPerformance.Fertility.ToString();
            this.performanceLabeles[5].Content = "Research Rate: +" + this.currentPerformance.ResearchRate.ToString() + "%";
            this.performanceLabeles[6].Content = "Growth: " + this.currentPerformance.Growth.ToString();
            this.performanceLabeles[7].Content = "Wealth: " + this.currentPerformance.Wealth.ToString();
            // Zmiana kolory w zależności czy konflikt wystąpił czy nie.
            if (this.currentPerformance.Conflict)
            {
                foreach (Label label in this.performanceLabeles)
                    label.Foreground = Brushes.Red;
            }
            else
            {
                foreach (Label label in this.performanceLabeles)
                    label.Foreground = Brushes.Black;
            }
        }
    }
}
