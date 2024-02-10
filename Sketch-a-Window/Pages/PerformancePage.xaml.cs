using Sketch_a_Window.Scripts;
using Windows.UI.Xaml.Controls;

namespace Sketch_a_Window.Pages
{
    public sealed partial class PerformancePage : Page
    {
        // Constructor
        // ======================================================================
        // ======================================================================
        public PerformancePage()
        {
            //Initialize ComponentS
            this.InitializeComponent();

            //Setup
            Setup();
        }



        // Setup
        // ======================================================================
        // ======================================================================
        private void Setup()
        {
            //Set Display Asleep's ComboBox Selected Index Value
            cboxDisplayAsleep.SelectedIndex = LocalSettings.ValidateValue("DisplayAsleepIndex") ? (int)LocalSettings.GetValue("DisplayAsleepIndex") : 0;

            //Set Laptop on Battery's ComboBox Selected Index Value
            cboxLaptopOnBattery.SelectedIndex = LocalSettings.ValidateValue("OnBatteryIndex") ? (int)LocalSettings.GetValue("OnBatteryIndex") : 0;
        }



        // Event Handlers
        // ======================================================================
        // ======================================================================
        private void cboxDisplayAsleep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Set Local Setting's Display Asleep Index Value to Display Asleep's ComboBox Selected Index
            LocalSettings.SetValue("DisplayAsleepIndex", cboxDisplayAsleep.SelectedIndex);

            //Set Local Setting's Display Asleep Value to Display Asleep's Selected ComboBox Value
            LocalSettings.SetValue("DisplayAsleep", ((ComboBoxItem)cboxDisplayAsleep.SelectedItem).Content.ToString().ToLower());
        }

        private void cboxLaptopOnBattery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Set Local Setting's On Battery Index Value to Laptop on Battery's ComboBox Selected Index
            LocalSettings.SetValue("OnBatteryIndex", cboxLaptopOnBattery.SelectedIndex);

            //Set Local Setting's On Battery Value to Laptop on Battery's Selected ComboBox Value
            LocalSettings.SetValue("OnBattery", ((ComboBoxItem)cboxLaptopOnBattery.SelectedItem).Content.ToString().ToLower());
        }
    }
}