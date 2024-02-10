using System;
using System.IO;
using WPF.Models;
using WPF.Scripts;
using System.Windows;
using Microsoft.Win32;
using System.Reflection;
using System.Windows.Controls;

namespace WPF.ViewModels
{
    public class WallpaperViewModel : WallpaperModel
    {
        // Constructor
        // ======================================================================
        // ======================================================================
        public WallpaperViewModel(Grid gwallpaper)
        {
            //Set ParentGrid
            ParentGrid = gwallpaper;
        }



        // Event Handlers
        // ======================================================================
        // ======================================================================
        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            //Set Content's Position To 0
            Content.Position = TimeSpan.FromSeconds(0);
        }



        #region Methods
        // Import
        // ======================================================================
        // ======================================================================
        public void Import()
        {
            //Variables
            bool isImported = false;

            //Validate Import
            if (ValidateImport(out string path))
            {
                //Get Video Preview
                Grabber.GetVideoPreview(path);

                //Initialize File Path
                string filepath = $"{LocalSettings.GetValue("VideosFolder")}/{LocalSettings.GetValue("WallpaperIndex")}{Path.GetExtension(path)}";

                //Validate File Path
                if (!File.Exists(filepath))
                {
                    //Copy Video File to Local Wallpaper Videos Folder
                    File.Copy(path, filepath);

                    //Set isImported to True
                    isImported = true;
                }

                //Set Local Setting's WallpaperImported Value to isImported Value
                LocalSettings.SetValue("WallpaperImported", isImported);
            }
        }


        // Create
        // ======================================================================
        // ======================================================================
        public void Create(bool ismodechange = false)
        {
            //Validate Source and Content
            if (File.Exists(Source) && Content == null)
            {
                //Create Media Element and Assign it into Content
                Content = new MediaElement()
                {
                    Name = "Content",
                    Source = new Uri(Source),
                    LoadedBehavior = MediaState.Manual,
                    Position = Position,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                //Set Event Handlers
                Content.MediaEnded += MediaElement_MediaEnded;

                //Add Content to Parent Grid
                ParentGrid.Children.Add(Content);

                //Refresh Settings
                SettingsManager.Set(true);

                //Check if the Power Mode has not been Changed
                if (!ismodechange)
                {
                    //Set Settings Manager's AppMode Variable to String.Empty
                    SettingsManager.AppMode = string.Empty;

                    //Set Power Mode
                    SettingsManager.PowerModeChanged(new PowerModeChangedEventArgs(PowerModes.Resume));
                }
                else
                {
                    //Play Content
                    Content.Play();
                }
            }
        }


        // Delete
        // ======================================================================
        // ======================================================================
        public void Delete()
        {
            //Validate Content
            if (Content != null)
            {
                //Get Position of Content
                Position = Content.Position;

                //Remove Content from Main Window's Grid
                ParentGrid.Children.Remove(Content);

                //Close Content
                Content.Close();

                //Set Content to Null
                Content = null;

                //Refresh Desktop
                WPFManager.RefreshDesktop();
            }
        }


        // Reset
        // ======================================================================
        // ======================================================================
        public void Reset(bool ismodechange = false)
        {
            //Delete Wallpaper
            Delete();

            //Create Wallpaper
            Create(ismodechange);
        }


        // Unload
        // ======================================================================
        // ======================================================================
        public void Unload()
        {
            //Check if the Local Setting's Unload Wallpaper Value has been Set to True
            if (LocalSettings.ValidateValue("UnloadWallpaper") && (bool)LocalSettings.GetValue("UnloadWallpaper"))
            {
                //Delete Wallpaper
                Delete();

                //Reset Content's Position
                Position = new TimeSpan();

                //Set Local Setting's Unload Wallpaper Value to False
                LocalSettings.SetValue("UnloadWallpaper", false);
            }
        }


        // Set Source
        // ======================================================================
        // ======================================================================
        public void SetSource()
        {
            //Validate Source
            if (ValidateSource(out string path))
            {
                //Set Source
                Source = path;

                //Reset Wallpaper
                Reset();
            }
        }
        #endregion Methods



        #region Extensions
        // Media State
        // ======================================================================
        // ======================================================================
        public MediaState GetMediaState()
        {
            //Validate Content
            if (Content != null)
            {
                //Source: https://stackoverflow.com/a/16819598
                //Get Media Element Helper
                FieldInfo hlp = typeof(MediaElement).GetField("_helper", BindingFlags.NonPublic | BindingFlags.Instance);

                //Get Content Fields
                object helperObject = hlp.GetValue(Content);

                //Get Content's Current State
                FieldInfo stateField = helperObject.GetType().GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);

                //Get Content's Media State
                MediaState mediastate = (MediaState)stateField.GetValue(helperObject);

                //Return Media State
                return mediastate;
            }
            else
            {
                //Return Default Media State
                return MediaState.Close;
            }
        }


        // Import
        // ======================================================================
        // ======================================================================
        private bool ValidateImport(out string path)
        {
            //Check if the Local Setting's NewWallpaper Value has Been Changed
            if ((bool)LocalSettings.GetValue("isNewWallpaper"))
            {
                //Set Local Setting's isNewWallpaper Value to False
                LocalSettings.SetValue("isNewWallpaper", false);

                //Set Path
                path = LocalSettings.GetValue("NewWallpaper").ToString();

                //Return True
                return true;
            }

            //Set path to String.Empty
            path = string.Empty;

            //Return False
            return false;
        }


        // Source
        // ======================================================================
        // ======================================================================
        private bool ValidateSource(out string path)
        {
            //Validate Local Setting's NewSource File Path, and Check if the NewSource has Been Changed
            if(File.Exists((string)LocalSettings.GetValue("NewSource")) && (((string)LocalSettings.GetValue("NewSource")) != ((string)LocalSettings.GetValue("OldSource"))))
            {
                //Set Local Setting's Old Source to New Source
                LocalSettings.SetValue("OldSource", LocalSettings.GetValue("NewSource"));

                //Set Path
                path = LocalSettings.GetValue("NewSource").ToString();

                //Return True
                return true;
            }

            //Set path to String.Empty
            path = string.Empty;

            //Return False
            return false;
        }
        #endregion Extensions
    }
}