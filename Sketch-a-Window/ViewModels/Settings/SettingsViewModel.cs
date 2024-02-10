using System;
using Windows.Foundation;
using Sketch_a_Window.Pages;
using Sketch_a_Window.Models;
using Sketch_a_Window.Scripts;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.WindowManagement.Preview;

namespace Sketch_a_Window.ViewModels
{
    public class SettingsViewModel : SettingsModel
    {
        // Event Handlers
        // ======================================================================
        // ======================================================================
        private void SettingsWindow_Closed(AppWindow sender, AppWindowClosedEventArgs args)
        {
            //Set isWindowActive to False
            isWindowActive = false;
        }



        #region Methods
        // Window
        // ======================================================================
        // ======================================================================
        public async void Create(int width = 650, int height = 700)
        {
            //Check if a Settings Window is Already Open
            if (isWindowActive == false)
            {
                //Create Settings Window
                AppWindow settingsWindow = await AppWindow.TryCreateAsync();

                //Create Frame
                Content = new Frame();

                //Set Requested Theme
                SetRequestedTheme();

                //Navigate to Settings Page within Content Frame
                Content.Navigate(typeof(SettingsPage), null);

                //Set Settings Window's Content to Content
                ElementCompositionPreview.SetAppWindowContent(settingsWindow, Content);

                //Attempt to Show the Settings Window
                bool shown = await settingsWindow.TryShowAsync();

                //Set Size of Settings Window
                WindowManagementPreview.SetPreferredMinSize(settingsWindow, new Size(width, height));
                settingsWindow.RequestSize(new Size(width, height));

                //Get the Application's Main Window Resolution
                GetAppResolution(out DisplayRegion displayregion, out double mainwidth, out double mainheight);

                //Get the Offset for the Settings Window
                GetWindowOffset(width, height, mainwidth, mainheight, out int hoffset, out int voffset);

                //Move Settings Window to Display Offset (Center of Main Window)
                settingsWindow.RequestMoveRelativeToDisplayRegion(displayregion, new Point(hoffset, voffset));

                //Set Event Handler
                settingsWindow.Closed += SettingsWindow_Closed;

                //Set isWindowActive to True
                isWindowActive = true;
            }
        }


        // Theme
        // ======================================================================
        // ======================================================================
        public static void SetRequestedTheme()
        {
            //Set Requested Theme
            Content.RequestedTheme = ApplicationTheme.Theme;
        }
        #endregion Methods



        #region Extension
        // Application Resolution
        // ======================================================================
        // ======================================================================
        private void GetAppResolution(out DisplayRegion displayregion, out double mainwidth, out double mainheight)
        {
            //Get Display Region of Main Window
            displayregion = ApplicationView.GetForCurrentView().GetDisplayRegions()[0];

            //Get the Main Window's Resolution
            mainwidth = displayregion.WorkAreaSize.Width;
            mainheight = displayregion.WorkAreaSize.Height;
        }


        // Settings Offset
        // ======================================================================
        // ======================================================================
        private void GetWindowOffset(int width, int height, double mainwidth, double mainheight, out int hoffset, out int voffset)
        {
            //Calculate Display Offset For Settings Window (Extra 20 to Accomadate For Padding)
            hoffset = ((int)(mainwidth - width - 20)) / 2;
            voffset = ((int)(mainheight - height)) / 2;
        }
        #endregion Extension
    }
}