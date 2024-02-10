using System;
using WPF.Scripts;
using System.Windows;
using WPF.ViewModels;
using System.Windows.Threading;

namespace WPF
{
    public partial class MainWindow : Window
    {
        #region Variables
        // Wallpaper View Model
        // ======================================================================
        // ======================================================================
        private WallpaperViewModel vmWallpaper;


        // Period Dispatcher
        // ======================================================================
        // ======================================================================
        private DispatcherTimer PeriodDispatcher = new DispatcherTimer();
        #endregion Variables



        // Constructor
        // ======================================================================
        // ======================================================================
        public MainWindow() { InitializeComponent(); }



        // Setup
        // ======================================================================
        // ======================================================================
        private void Setup()
        {
            //Create Wallpaper View Model
            vmWallpaper = new WallpaperViewModel(ParentGrid);

            //Setup Settings Manager
            SettingsManager.Setup(vmWallpaper);

            //Set Parent Window
            WPFManager.ParentWindow();

            //Set Windows Properties
            SetWindowProperties();

            //Setup Dispatcher
            SetupDispatcher();
        }



        #region Event Handlers
        // Window
        // ======================================================================
        // ======================================================================
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Setup
            Setup();
        }


        // Dispatcher
        // ======================================================================
        // ======================================================================
        private void PeriodDispatcher_Tick(object sender, EventArgs e)
        {
            //Validate Application's Close State (If UWP Application is Shutting Down. Close the WPF Application).
            CloseApp();

            //Check if the Local Setting's NewSource Value has been Changed. If so, Update Content Source.
            vmWallpaper.SetSource();

            //Check if the Local Setting's Unload Wallpaper Value has been Changed. If so, Unload Content.
            vmWallpaper.Unload();

            //Check if the Local Setting's NewWallpaper Value has been Changed. If so, Import the New Wallpaper to the Application
            vmWallpaper.Import();

            //Validate and Set Settings
            SettingsManager.Set();
        }
        #endregion Event Handlers



        #region Methods
        // Application
        // ======================================================================
        // ======================================================================
        private void CloseApp()
        {
            //Get Local Setting's Closed Value
            bool isclosed = LocalSettings.ValidateValue("Closed") ? (bool)LocalSettings.GetValue("Closed") : false;

            //Validate Application's Close State (If UWP Application is Shutting Down. Close the WPF Application).
            if (isclosed)
            {
                //Close Window
                this.Close();

                //Refresh Desktop
                WPFManager.RefreshDesktop();
            }
        }


        // Window
        // ======================================================================
        // ======================================================================
        private void SetWindowProperties()
        {
            //Assign Size
            this.Height = SystemParameters.PrimaryScreenHeight;
            this.Width = SystemParameters.PrimaryScreenWidth;

            //Assign Position
            this.Top = SystemParameters.VirtualScreenTop;
            this.Left = SystemParameters.VirtualScreenLeft;
        }


        // Dispatcher
        // ======================================================================
        // ======================================================================
        private void SetupDispatcher()
        {
            //Setup Dispatcher
            PeriodDispatcher.Tick += new EventHandler(PeriodDispatcher_Tick);
            PeriodDispatcher.Interval = new TimeSpan(0, 0, 0, 0, 300);
            PeriodDispatcher.Start();
        }
        #endregion Methods
    }
}