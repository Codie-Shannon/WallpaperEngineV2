using Windows.UI.Xaml;
using Sketch_a_Window.Scripts;
using Windows.UI.Xaml.Controls;
using Sketch_a_Window.ViewModels;
using ApplicationTheme = Sketch_a_Window.Scripts.ApplicationTheme;

namespace Sketch_a_Window.Pages
{
    public sealed partial class GeneralPage : Page
    {
        // Constructor
        // ======================================================================
        // ======================================================================
        public GeneralPage()
        {
            //Initialize Components
            this.InitializeComponent();

            //Setup
            Setup();
        }



        // Setup
        // ======================================================================
        // ======================================================================
        private void Setup()
        {
            //Set Selected Item of Theme's ComboBox
            cboxTheme.SelectedIndex = ApplicationTheme.Theme == ApplicationTheme.DarkTheme ? 0 : 1;

            //Set Audio Output CheckBox
            cbAudioOutput.IsChecked = LocalSettings.ValidateValue("AudioOutput") ? (bool)LocalSettings.GetValue("AudioOutput") : true;
        }



        // Event Handlers
        // ======================================================================
        // ======================================================================
        private void cboxTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Set Theme
            SetTheme();
        }

        private void cbAudioOutput_Click(object sender, RoutedEventArgs e)
        {
            //Set Local Setting's Audio Output Value
            LocalSettings.SetValue("AudioOutput", cbAudioOutput.IsChecked);
        }



        // Methods
        // ======================================================================
        // ======================================================================
        private void SetTheme()
        {
            //Get Current Window's Content
            FrameworkElement window = (FrameworkElement)Window.Current.Content;

            //Get Selected Theme
            ElementTheme theme = cboxTheme.SelectedIndex == 0 ? ApplicationTheme.DarkTheme : ApplicationTheme.LightTheme;

            //Set Selected Theme
            ApplicationTheme.Theme = theme;
            window.RequestedTheme = theme;
            SettingsViewModel.SetRequestedTheme();
        }



        // Was adding support for audio device selection. However, I couldn't get the WPF side to function.
        // ======================================================================
        // ======================================================================
        //private async void SetupAudioDeviceComboBoxAsync()
        //{
        //    //Default Device
        //    DeviceInformation defaultdevice = await DeviceInformation.CreateFromIdAsync(MediaDevice.GetDefaultAudioRenderId(AudioDeviceRole.Default));

        //    //All Devices
        //    DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(MediaDevice.GetAudioRenderSelector());

        //    //Populate Audio Device ComboBox
        //    PopulateAudioDeviceComboBox(devices);

        //    //Get Selected Audio Device
        //    string selectedAudioDevice = LocalSettings.ValidateValue("AudioDevice") ? (string)LocalSettings.GetValue("AudioDevice") : defaultdevice.Name;

        //    //Set Selected Audio Device
        //    SetSelectedAudioDevice(selectedAudioDevice);
        //}

        //private void PopulateAudioDeviceComboBox(DeviceInformationCollection devices)
        //{
        //    //Loop through Audio Devices
        //    foreach (DeviceInformation item in devices)
        //    {
        //        //Populate Audio Device ComboBox with Audio Device Names
        //        cboxAudioDevices.Items.Add(new ComboBoxItem() { Content = item.Name });
        //    }
        //}

        //private void SetSelectedAudioDevice(string selectedAudioDevice)
        //{
        //    //Validate Selected Audio Device
        //    if (cboxAudioDevices.Items.Any(i => ((ComboBoxItem)i).Content.ToString() == (string)LocalSettings.GetValue("AudioDevice")))
        //    {
        //        //Get Selected Audio Device's ComboBoxItem
        //        ComboBoxItem item = (ComboBoxItem)cboxAudioDevices.Items.Single(i => ((ComboBoxItem)i).Content.ToString() == selectedAudioDevice);

        //        //Set Audio Device ComboBox's Selected Index
        //        cboxAudioDevices.SelectedIndex = cboxAudioDevices.Items.IndexOf(item);
        //    }
        //    else
        //    {
        //        //Set Audio Device ComboBox's Selected Index
        //        cboxAudioDevices.SelectedIndex = 0;

        //        //Set Local Setting's Audio Device Value
        //        LocalSettings.SetValue("AudioDevice", ((ComboBoxItem)cboxAudioDevices.SelectedItem).Content);
        //    }
        //}
    }
}