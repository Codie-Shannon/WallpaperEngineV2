using System;
using Windows.UI;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using System.Threading.Tasks;
using Sketch_a_Window.Models;
using Sketch_a_Window.Scripts;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using SelectionMode = Sketch_a_Window.Scripts.SelectionMode;

namespace Sketch_a_Window.Pages
{
    public sealed partial class InstalledPage : Page
    {
        #region Variables
        // Observable Collections
        // ======================================================================
        // ======================================================================
        private ObservableCollection<WallpaperModel> collection = new ObservableCollection<WallpaperModel>();
        private ObservableCollection<WallpaperModel> activewallpapers = new ObservableCollection<WallpaperModel>();


        // Wallpaper
        // ======================================================================
        // ======================================================================
        private int counter;


        // Selection
        // ======================================================================
        // ======================================================================
        private Button selectedItem;
        private WallpaperModel selectedWallpaper;
        private bool isSelectionChange = false;
        #endregion Variables



        // Constructor
        // ======================================================================
        // ======================================================================
        public InstalledPage()
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
            //Set Volume Slider Value
            sVolume.Value = LocalSettings.ValidateValue("Volume") ? (double)LocalSettings.GetValue("Volume") : 50;

            //Set Playback Rate Slider Value
            sPlaybackRate.Value = LocalSettings.ValidateValue("PlaybackRate") ? (double)LocalSettings.GetValue("PlaybackRate") : 50;

            //Set Playback Rate Slider Minimum Value
            sPlaybackRate.Minimum = 5;

            //Set Flip CheckBox Value
            cbFlip.IsChecked = LocalSettings.ValidateValue("Flip") ? (bool)LocalSettings.GetValue("Flip") : true;

            //Set Alignment's ComboBox Selected Index Value
            cboxAlignment.SelectedIndex = LocalSettings.ValidateValue("AlignmentIndex") ? (int)LocalSettings.GetValue("AlignmentIndex") : 0;
        }



        #region Event Handlers
        // Import
        // ======================================================================
        // ======================================================================
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            //Import Wallpaper
            Import();
        }


        // Remove
        // ======================================================================
        // ======================================================================
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            //Remove Wallpaper
            Remove();
        }


        // Selection
        // ======================================================================
        // ======================================================================
        private void ItemTemplate_Click(object sender, RoutedEventArgs e)
        {
            //Select New Wallpaper
            SetSelection(SelectionMode.Select, sender);
        }

        private void IcItems_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //Check if the Items Control was Tapped
            if (e.OriginalSource.GetType() == typeof(ItemsWrapGrid))
            {
                //Unselect Wallpaper
                SetSelection(SelectionMode.Unselect);
            }
        }


        // Search
        // ======================================================================
        // ======================================================================
        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Search
            Search();
        }


        // Volume
        // ======================================================================
        // ======================================================================
        private void sVolume_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            //Set Local Setting's Volume Value to Volume Slider Value
            LocalSettings.SetValue("Volume", sVolume.Value);
        }


        // Playback Rate
        // ======================================================================
        // ======================================================================
        private void sPlaybackRate_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            //Set Local Setting's Playback Rate Value to Playback Rate's Slider Value
            LocalSettings.SetValue("PlaybackRate", sPlaybackRate.Value);
        }


        // Flip
        // ======================================================================
        // ======================================================================
        private void cbFlip_Click(object sender, RoutedEventArgs e)
        {
            //Set Local Setting's Flip Value to Flip CheckBox Boolean Value
            LocalSettings.SetValue("Flip", cbFlip.IsChecked);
        }


        // Alignment
        // ======================================================================
        // ======================================================================
        private void cboxAlignment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Set Local Setting's Alignment Index Value to Alignment's ComboBox Selected Index
            LocalSettings.SetValue("AlignmentIndex", cboxAlignment.SelectedIndex);

            //Set Local Setting's Alignment Value to Alignment's Selected ComboBox Value
            LocalSettings.SetValue("Alignment", ((ComboBoxItem)cboxAlignment.SelectedItem).Content.ToString().ToLower());
        }
        #endregion Event Handlers



        #region Methods
        // Import
        // ======================================================================
        // ======================================================================
        private async void Import()
        {
            //Open File Picker and Allow User to Select Video File to Generate a StorageFile.
            StorageFile file = await OpenFileBrowser.OpenFilePickerAsync(new string[] { "mp4", "mkv", "avi", "mov" });

            //Check if the Wallpaper Already Exists within the Application
            bool isExists = collection.Any(i => i.OriginalPath == file.Path);

            //Validate Wallpaper
            if (isExists)
            {
                //Show Exists Message
                MessageBox.Show("Error: Wallpaper Exists within Application.");
            }
            else if (file != null)
            {
                //Disable Import Wallpaper Button
                btnImportWallpaper.IsEnabled = false;

                //Increment Wallpaper Counter
                counter++;

                //Set Local Setting's WallpaperImported Value to False
                LocalSettings.SetValue("WallpaperImported", false);

                //Set Local Setting's New Wallpaper Values
                LocalSettings.SetValue("isNewWallpaper", true);
                LocalSettings.SetValue("NewWallpaper", file.Path);
                LocalSettings.SetValue("WallpaperIndex", counter);

                //Delay Task by 5 Seconds
                await Task.Delay(5000);

                //Check if the New Wallpaper was Imported to the Application's Local Folder
                if ((bool)LocalSettings.GetValue("WallpaperImported"))
                {
                    //Create Paths
                    string video = $"{LocalSettings.GetValue("VideosFolder")}\\{counter}{file.FileType}";
                    string preview = $"{LocalSettings.GetValue("ImagesFolder")}\\{counter}.png";

                    //Add Wallpaper to Observable Collection
                    collection.Add(new WallpaperModel() { Id = counter, Name = file.DisplayName, FilePath = video, OriginalPath = file.Path, CoverImage = preview });

                    //Refresh Search
                    Search();
                }
                else
                {
                    //Decrement Wallpaper Counter
                    counter--;

                    //Show Error Message
                    MessageBox.Show("An Error Occured While Attempting to Import the Wallpaper.");
                }

                //Enable Import Wallpaper Button
                btnImportWallpaper.IsEnabled = true;
            }
        }


        // Remove
        // ======================================================================
        // ======================================================================
        private async void Remove()
        {
            //Variables
            bool isRemove = false;

            //Validate if a Wallpaper is Currently Selected
            if (selectedWallpaper != null)
            {
                //Wallpaper Removal Confirmation MessageBox
               isRemove = await MessageBox.ShowYesNo($"Are you sure you would like to remove the wallpaper of {selectedWallpaper.Name} from the application?");
            }
            else
            {
                //Show Error Message
                MessageBox.Show("Please select a wallpaper to remove.");
            }

            //Validate Remove
            if (isRemove)
            {
                //Unload Wallpaper
                Items.Unload(LoadedMode.Unloaded);

                //Remove Item
                RemoveItem(collection);

                //Unselect Wallpaper
                Items.SelectionChange(SelectionMode.Unselect, SelectionType.Item, activewallpapers, selectedItem, out selectedItem, selectedWallpaper, out selectedWallpaper);

                //Hide Information Pane
                gInformationPane.Visibility = Visibility.Collapsed;
            }
        }

        private void RemoveItem(ObservableCollection<WallpaperModel> collection)
        {
            //Loop through Items in collection Observable Collection
            for (int i = 0; i < collection.Count; i++)
            {
                //Validate Current Looped Item
                if(selectedWallpaper.FilePath == collection[i].FilePath)
                {
                    //Remove Current Item from collection Observable Collection
                    collection.RemoveAt(i);
                }
            }

            //Refresh Search
            Search();
        }


        // Selection
        // ======================================================================
        // ======================================================================
        private async void SetSelection(SelectionMode mode, object sender = null)
        {
            //Check if a Selection Change is Currently in Progress
            if (!isSelectionChange)
            {
                //Set isSelectionChange Variable to True
                isSelectionChange = true;

                //Unload Wallpaper
                Items.Unload(mode == SelectionMode.Unselect ? LoadedMode.Unloaded : LoadedMode.Loaded);

                //Unselect Current Wallpaper
                Items.SelectionChange(SelectionMode.Unselect, SelectionType.Wallpaper, activewallpapers, selectedItem, out selectedItem, selectedWallpaper, out selectedWallpaper);

                //Validate Selection Mode
                if (mode == SelectionMode.Select)
                {
                    //Show Selection Blocker
                    gBlockSelection.Visibility = Visibility.Visible;

                    //Select New Wallpaper
                    Items.SelectionChange(SelectionMode.Select, SelectionType.Wallpaper, activewallpapers, (Button)sender, out selectedItem, selectedWallpaper, out selectedWallpaper);

                    //Set Name
                    tbName.Text = selectedWallpaper.Name;

                    //Set Cover Image
                    imgCover.Source = new BitmapImage(new Uri(selectedWallpaper.CoverImage, UriKind.Absolute));

                    //Show Information Pane
                    gInformationPane.Visibility = Visibility.Visible;

                    //Delay Task by a Second
                    await Task.Delay(1000);

                    //Hide Selection Blocker
                    gBlockSelection.Visibility = Visibility.Collapsed;
                }
                else
                {
                    //Hide Information Pane
                    gInformationPane.Visibility = Visibility.Collapsed;
                }

                //Set isSelectionChange Variable to False
                isSelectionChange = false;
            }
        }


        // Search
        // ======================================================================
        // ======================================================================
        private async void Search()
        {
            //Unselect Selected Item
            Items.SelectionChange(SelectionMode.Unselect, SelectionType.Item, activewallpapers, selectedItem, out selectedItem, selectedWallpaper, out selectedWallpaper);

            //Clear Active Wallpapers Observable Collection
            activewallpapers.Clear();

            //Loop through collection Observable Collection
            foreach (WallpaperModel item in collection)
            {
                //Check if the Current Looped item's Name Contains the tbSearch Text Value
                if (item.Name.ToLower().Contains(tbSearch.Text.ToLower()))
                {
                    //Add item to Observable Collection
                    activewallpapers.Add(item);

                    //Check if the Current Looped item Matches the selectedWallpaper
                    if (selectedWallpaper != null && item.Id == selectedWallpaper.Id)
                    {
                        //Delay Task by 1 Millisecond
                        await Task.Delay(1);

                        //Set selectedItem to selectedWallpaper's Button Element
                        selectedItem = Items.GetItem(icItems, selectedWallpaper.Id);

                        //Check if the selectedItem is Equal to Null
                        if (selectedItem != null)
                        {
                            //Set Border Brush
                            selectedItem.BorderBrush = new SolidColorBrush(Colors.Red);
                        }
                    }
                }
            }
        }
        #endregion Methods
    }
}