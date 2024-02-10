using System;
using WPF.ViewModels;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Controls;
using PowerLineStatus = System.Windows.Forms.PowerLineStatus;

namespace WPF.Scripts
{
    public class SettingsManager
    {
        #region Variables
        // Wallpaper View Model
        // ======================================================================
        // ======================================================================
        private static WallpaperViewModel vmWallpaper;


        // Alignment View Model
        // ======================================================================
        // ======================================================================
        private static AlignmentViewModel vmAlignment;


        // Power Modes
        // ======================================================================
        // ======================================================================
        public static string AppMode;
        private static PowerModes Mode = PowerModes.Resume;
        private static PowerLineStatus ChargerStatus;
        private static bool isPowerModeChange = false;
        private static int stopTimer = 0;
        #endregion Variables



        // Setup
        // ======================================================================
        // ======================================================================
        public static void Setup(WallpaperViewModel wallpaperModel)
        {
            //Set Wallpaper View Model
            vmWallpaper = wallpaperModel;

            //Create New Alignment View Model
            vmAlignment = new AlignmentViewModel(vmWallpaper, vmWallpaper.Content);

            //Set Event Handlers
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChangedAsync;
        }



        // Event Handlers
        // ======================================================================
        // ======================================================================
        private static async void SystemEvents_PowerModeChangedAsync(object sender, PowerModeChangedEventArgs e)
        {
            //Check if a Power Mode Change is Currently in Progress
            if (isPowerModeChange == false)
            {
                //Set isPowerModeChange to True
                isPowerModeChange = true;

                //Power Mode Changed
                PowerModeChanged(e);

                //Delay Task by 1 Second
                await Task.Delay(1000);

                //Set isPowerModeChange to False
                isPowerModeChange = false;
            }
        }



        #region Methods
        // Settings
        // ======================================================================
        // ======================================================================
        public static void Set(bool isRefresh = false)
        {
            //Validate Content
            if (vmWallpaper.Content != null)
            {
                //Set Alignment View Model's Content to Wallpaper View Model's Content
                vmAlignment.SetContent(vmWallpaper.Content);

                //Get Settings
                Get(out double volume, out double playbackrate, out bool isflipped, out bool islastflipped, out string alignment, out bool isaudiooutput, out string displayasleep, out string lastdisplayasleep, out string onbattery, out string lastonbattery);

                //Apply Settings
                Apply(isRefresh, volume, playbackrate, isflipped, islastflipped, alignment, isaudiooutput, displayasleep, lastdisplayasleep, onbattery, lastonbattery);
            }
        }

        private static void Get(out double volume, out double playbackrate, out bool isflipped, out bool islastflipped, out string alignment, out bool isaudiooutput, out string displayasleep, out string lastdisplayasleep, out string onbattery, out string lastonbattery)
        {
            // Settings Pane
            // =============================
            // Volume
            volume = LocalSettings.ValidateValue("Volume") ? (double)LocalSettings.GetValue("Volume") / 100 : 0.5;

            // Playback Rate
            playbackrate = LocalSettings.ValidateValue("PlaybackRate") ? (double)LocalSettings.GetValue("PlaybackRate") / 50 : 0.5;

            // Flip
            isflipped = LocalSettings.ValidateValue("Flip") ? (bool)LocalSettings.GetValue("Flip") : false;
            islastflipped = LocalSettings.ValidateValue("LastFlip") ? (bool)LocalSettings.GetValue("LastFlip") : true;

            // Alignment
            alignment = LocalSettings.ValidateValue("Alignment") ? (string)LocalSettings.GetValue("Alignment") : "Stretch";


            // General Page
            // =============================
            // Audio Output
            isaudiooutput = LocalSettings.ValidateValue("AudioOutput") ? (bool)LocalSettings.GetValue("AudioOutput") : true;


            // Performance Page
            // =============================
            // Display Asleep
            displayasleep = LocalSettings.ValidateValue("DisplayAsleep") ? (string)LocalSettings.GetValue("DisplayAsleep") : "Pause";
            lastdisplayasleep = LocalSettings.ValidateValue("LastDisplayAsleep") ? (string)LocalSettings.GetValue("LastDisplayAsleep") : "";

            // Laptop on Battery
            onbattery = LocalSettings.ValidateValue("OnBattery") ? (string)LocalSettings.GetValue("OnBattery") : "Continue Running";
            lastonbattery = LocalSettings.ValidateValue("LastOnBattery") ? (string)LocalSettings.GetValue("LastOnBattery") : "";
        }

        private static void Apply(bool isrefresh, double volume, double playbackrate, bool isflipped, bool islastflipped, string alignment, bool isaudiooutput, string displayasleep, string lastdisplayasleep, string onbattery, string lastonbattery)
        {
            // Settings Pane
            // =============================
            // Volume
            vmWallpaper.Content.Volume = volume != vmWallpaper.Content.Volume || isrefresh ? volume : vmWallpaper.Content.Volume;

            // Playback Rate
            vmWallpaper.Content.SpeedRatio = playbackrate != vmWallpaper.Content.SpeedRatio || isrefresh ? playbackrate : vmWallpaper.Content.SpeedRatio;

            // Flip
            Flip(isrefresh, isflipped, islastflipped);

            // Alignment
            vmAlignment.Alignment(alignment);


            // General Page
            // =============================
            // Set Audio Output
            vmWallpaper.Content.IsMuted = !isaudiooutput;


            // Performance Page
            // =============================
            // Validate Power Modes
            if ((displayasleep != lastdisplayasleep) || (onbattery != lastonbattery))
            {
                //Set Local Setting's LastOnBattery and LastDisplayAsleep Values
                LocalSettings.SetValue("LastOnBattery", onbattery);
                LocalSettings.SetValue("LastDisplayAsleep", displayasleep);

                //Set Power Mode
                PowerModeChanged(new PowerModeChangedEventArgs(PowerModes.Resume));
            }
        }


        // Power Mode
        // ======================================================================
        // ======================================================================
        public static void PowerModeChanged(PowerModeChangedEventArgs e)
        {
            //Get Power Mode
            GetPowerMode(e);

            //Set Application Mode
            SetApplicationMode();
        }

        private static async void SetApplicationMode()
        {
            //Validate Power Mode
            if (Mode == PowerModes.Resume || Mode == PowerModes.StatusChange)
            {
                //Validate Charger Status
                if (ChargerStatus == PowerLineStatus.Offline)
                {
                    //Get On Battery Mode
                    string onbattery = LocalSettings.ValidateValue("OnBattery") ? LocalSettings.GetValue("OnBattery").ToString() : "continue running";

                    //Check if the Application's Current Mode is Stop and if the On Battery Mode is Not Stop
                    if (AppMode == "stop (free memory)" && AppMode != onbattery)
                    {
                        //Unset Mode
                        await SetModeAsync(AppMode, false);
                    }

                    //Set Stop Timer
                    stopTimer = 0;

                    //Validate App Mode Change
                    if (AppMode != onbattery || AppMode == "pause" && vmWallpaper.GetMediaState() != MediaState.Pause)
                    {
                        //Set App Mode
                        AppMode = onbattery;

                        //Set Mode
                        await SetModeAsync(AppMode, true);
                    }
                }
                else if (ChargerStatus == PowerLineStatus.Online && AppMode != "continue running")
                {
                    //Set Mode
                    await SetModeAsync(AppMode, false);

                    //Set App Mode
                    AppMode = "continue running";
                }
            }
            else if (Mode == PowerModes.Suspend)
            {
                //Set Stop Timer
                stopTimer = 5000;

                //Get Display Asleep Mode
                AppMode = LocalSettings.ValidateValue("DisplayAsleep") ? LocalSettings.GetValue("DisplayAsleep").ToString() : "pause";

                //Set Mode
                await SetModeAsync(AppMode, true);
            }
        }

        private static async Task SetModeAsync(string value, bool isModeOn)
        {
            //Validate Content
            if (vmWallpaper.Content != null)
            {
                //Validate Mode
                if (value == "pause" && isModeOn)
                {
                    //Reset Wallpaper
                    vmWallpaper.Reset(true);

                    //Hide Content
                    vmWallpaper.Content.Visibility = Visibility.Visible;

                    //Delay Task by 0.5 Seconds
                    await Task.Delay(500);

                    //Validate Content
                    if (vmWallpaper.Content != null)
                    {
                        //Pause Content
                        vmWallpaper.Content.Pause();
                    }
                }
                else if (value == "pause" && !isModeOn)
                {
                    //Reset Wallpaper
                    vmWallpaper.Reset(true);
                }
                else if (value == "stop (free memory)" && isModeOn)
                {
                    //Stop Content
                    await StopAsync(true);
                }
                else if (value == "stop (free memory)" && !isModeOn)
                {
                    //Resume Content
                    await StopAsync(false);
                }
                else if (vmWallpaper.GetMediaState() != MediaState.Play)
                {
                    //Reset Wallpaper
                    vmWallpaper.Reset(true);
                }
            }
        }


        // Stop
        // ======================================================================
        // ======================================================================
        private static async Task StopAsync(bool isOn)
        {
            //Validate Content
            if (vmWallpaper.Content != null)
            {
                //Validate Stop
                if (isOn)
                {
                    //Stop Content
                    vmWallpaper.Content.Stop();

                    //Reset Content's Position
                    vmWallpaper.Position = new TimeSpan();

                    //Hide Content
                    vmWallpaper.Content.Visibility = Visibility.Collapsed;
                }
                else
                {
                    //Delay Task
                    await Task.Delay(stopTimer);

                    //Reset Wallpaper
                    vmWallpaper.Reset(true);
                }
            }
        }


        // Flip
        // ======================================================================
        // ======================================================================
        private static void Flip(bool isrefresh, bool isflipped, bool islastflipped)
        {
            //Set the Render Transform Origin (Anchor Point) for the Content to the Center
            vmWallpaper.Content.RenderTransformOrigin = new Point(0.5, 0.5);

            //Validate Flip
            if (isflipped && (!islastflipped || isrefresh))
            {
                //Set Scale Transform of Content
                vmWallpaper.Content.RenderTransform = new ScaleTransform(-1, 1);
            }
            else if (!isflipped && (islastflipped || isrefresh))
            {
                //Set Scale Transform of Content
                vmWallpaper.Content.RenderTransform = new ScaleTransform(1, 1);
            }

            //Set Local Setting's LastFlip Value to isflipped Value
            LocalSettings.SetValue("LastFlip", isflipped);
        }
        #endregion Methods



        // Extensions
        // ======================================================================
        // ======================================================================
        private static void GetPowerMode(PowerModeChangedEventArgs e)
        {
            //Get Laptop Charger Status
            ChargerStatus = SystemInformation.PowerStatus.PowerLineStatus;

            //Validate Power Mode
            if (e.Mode != PowerModes.StatusChange)
            {
                //Get Power Mode Status
                Mode = e.Mode;
            }
        }
    }
}