using System;
using Windows.UI.Xaml;
using Windows.Storage;
using Microsoft.AppCenter;
using System.Threading.Tasks;
using Sketch_a_Window.Scripts;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using Windows.ApplicationModel.Activation;

namespace Sketch_a_Window
{
    sealed partial class App : Application
    {
        // Variables
        // ======================================================================
        // ======================================================================
        private List<string> LocalFolders = new List<string>() { @"Wallpapers\Images", @"Wallpapers\Videos" };



        // Constructor
        // ======================================================================
        // ======================================================================
        public App()
        {
            //Initialize Components
            this.InitializeComponent();

            //Start AppCenter
            AppCenter.Start("f15e28ca-442e-47d1-acb7-281c8c9b18aa", typeof(Analytics), typeof(Crashes));

            //Reset Wallpaper Values
            LocalSettings.SetValue("NewWallpaper", string.Empty);
            LocalSettings.SetValue("isNewWallpaper", false);
            LocalSettings.SetValue("NewSource", string.Empty);
            LocalSettings.SetValue("OldSource", string.Empty);

            //Set Local Setting's Folder Locations
            LocalSettings.SetValue("LocalFolder", ApplicationData.Current.LocalFolder.Path);
            LocalSettings.SetValue("VideosFolder", $"{ApplicationData.Current.LocalFolder.Path}\\Wallpapers\\Videos");
            LocalSettings.SetValue("ImagesFolder", $"{ApplicationData.Current.LocalFolder.Path}\\Wallpapers\\Images");

            //Set Local Folders
            SetLocalFoldersAsync();
        }



        // Event Handlers
        // ======================================================================
        // ======================================================================
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            //Get Current Window's Content
            Frame rootFrame = Window.Current.Content as Frame;

            //Check if a Window is Active
            if (rootFrame == null)
            {
                //Create New Frame
                rootFrame = new Frame();

                //Set Event Handlers
                rootFrame.NavigationFailed += OnNavigationFailed;

                //Set Current Window's Content to rootFrame Variable
                Window.Current.Content = rootFrame;
            }

            //Check if the Application was Prelaunched
            if (e.PrelaunchActivated == false)
            {
                //Check if the Current Window Contains Content
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }

                //Ensure the Current Window is Active
                Window.Current.Activate();
            }
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            //Throw Exception
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }



        // Methods
        // ======================================================================
        // ======================================================================
        private async void SetLocalFoldersAsync()
        {
            //Delete Local Folders
            await DeleteLocalFoldersAsync();

            //Create Local Folders
            await CreateLocalFoldersAsync();
        }

        private async Task DeleteLocalFoldersAsync()
        {
            //Get Local Folders
            IReadOnlyList<StorageFolder> folders = await ApplicationData.Current.LocalFolder.GetFoldersAsync();

            //Loop through Folders List
            for (int i = 0; i < folders.Count; i++)
            {
                //Check if the Local Folder's Display Name is Equal to Wallpapers
                if (folders[i].DisplayName == "Wallpapers")
                {
                    //Delete Wallpapers Folder
                    await folders[i].DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
        }

        private async Task CreateLocalFoldersAsync()
        {
            //Loop through LocalFolders List
            foreach (string localfolder in LocalFolders)
            {
                //Create Local Folder
                await ApplicationData.Current.LocalFolder.CreateFolderAsync(localfolder);
            }
        }
    }
}