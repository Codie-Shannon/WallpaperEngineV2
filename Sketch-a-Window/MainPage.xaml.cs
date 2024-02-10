using Windows.UI.Xaml;
using Sketch_a_Window.Pages;
using Sketch_a_Window.Scripts;
using Windows.UI.Xaml.Controls;
using Sketch_a_Window.ViewModels;
using ApplicationTheme = Sketch_a_Window.Scripts.ApplicationTheme;

namespace Sketch_a_Window
{
    public sealed partial class MainPage : Page
    {
        #region Variables
        // WPF View Model
        // ======================================================================
        // ======================================================================
        private WPFViewModel vmWPF = new WPFViewModel();


        // Settings View Model
        // ======================================================================
        // ======================================================================
        private SettingsViewModel vmSettings = new SettingsViewModel();
        #endregion Variables



        // Constructor
        // ======================================================================
        // ======================================================================
        public MainPage()
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
            //Navigate To Default Page
            Navigation.NavigateToPage(fFrame, new InstalledPage(), nvNavigation, nviInstalled);

            //Set Root Window's Theme
            FrameworkElement root = (FrameworkElement)Window.Current.Content;
            root.RequestedTheme = ApplicationTheme.Theme;

            //Create WPF Application
            vmWPF.Create();
        }



        // Event Handlers
        // ======================================================================
        // ======================================================================
        private void NvNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            //Check if the Settings Button was Clicked
            if (args.IsSettingsInvoked)
            {
                //Create Settings Window
                vmSettings.Create();

                //Reselect Navigation View Item
                nvNavigation.SelectedItem = nviInstalled;
            }
        }
    }
}