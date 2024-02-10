using Windows.UI;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Sketch_a_Window.Models;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sketch_a_Window.Scripts
{
    public class Items
    {
        #region Item Selection
        // Selection Change
        // ======================================================================
        // ======================================================================
        public static void SelectionChange(SelectionMode selection, SelectionType type, ObservableCollection<WallpaperModel> activewallpapers, Button Item, out Button selectedItem, WallpaperModel wallpaper, out WallpaperModel selectedWallpaper)
        {
            //Validate Selection
            if (selection == SelectionMode.Select)
            {
                //Select Wallpaper
                Select(activewallpapers, Item, out selectedItem, out selectedWallpaper);
            }
            else if (selection == SelectionMode.Unselect && Item != null)
            {
                //Unselect Wallpaper
                Unselect(type, Item, out selectedItem, wallpaper, out selectedWallpaper);
            }
            else
            {
                //Unchange Wallpaper
                selectedItem = Item;
                selectedWallpaper = wallpaper;
            }
        }


        // Select
        // ======================================================================
        // ======================================================================
        private static void Select(ObservableCollection<WallpaperModel> activewallpapers, Button Item, out Button selectedItem, out WallpaperModel selectedWallpaper)
        {
            //Set selectedItem to Item
            selectedItem = Item;

            //Set Border Brush
            selectedItem.BorderBrush = new SolidColorBrush(Colors.Red);

            //Set Wallpaper
            selectedWallpaper = activewallpapers.Single(i => i.Id.ToString() == Item.Tag.ToString());

            //Set Local Setting's New Source Value to Selected Wallpaper's File Path
            LocalSettings.SetValue("NewSource", selectedWallpaper.FilePath);
        }


        // Unselect
        // ======================================================================
        // ======================================================================
        private static void Unselect(SelectionType type, Button Item, out Button selectedItem, WallpaperModel wallpaper, out WallpaperModel selectedWallpaper)
        {
            //Set Border Brush
            Item.BorderBrush = new SolidColorBrush(Colors.Gray);

            //Set selectedItem to Null
            selectedItem = null;

            //Validate SelectionType
            if (type == SelectionType.Wallpaper)
            {
                //Set selectedWallpaper to Null
                selectedWallpaper = null;
            }
            else
            {
                //Unchange selectedWallpaper
                selectedWallpaper = wallpaper;
            }
        }
        #endregion Item Selection



        #region Item Loading
        // Unload
        // ======================================================================
        // ======================================================================
        public static void Unload(LoadedMode loaded)
        {
            //Validate Loaded Mode
            if (loaded == LoadedMode.Unloaded)
            {
                //Unset Local Setting's Source Values
                LocalSettings.SetValue("OldSource", "");
                LocalSettings.SetValue("NewSource", "");

                //Set Load Setting's UnloadWallpaper Value to True
                LocalSettings.SetValue("UnloadWallpaper", true);
            }
        }
        #endregion Item Loading



        #region Get Elements
        // Get Items
        // ======================================================================
        // ======================================================================
        public static Button GetItem(ItemsControl icitems, int id)
        {
            //Get Visual Children from Items Control
            foreach (Button item in FindVisualChildren<Button>(icitems))
            {
                //Validate Item
                if (item != null && (int)item.Tag == id)
                {
                    //Return Item
                    return item;
                }
            }

            //Return Null
            return null;
        }


        // Find Visual Children
        // ======================================================================
        // ======================================================================
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            //Check if the Dependency Object has been Set
            if (depObj != null)
            {
                //Loop Through Visual Tree Helper Children of Dependency Object
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    //Get Visual Tree Helper Child at Position i of the Dependency Object
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    //Check if the Child Dependency Object has been Set
                    if (child != null && child is T)
                    {
                        //Cast Child Dependency Object to Type T and Return It
                        yield return (T)child;
                    }

                    //Loop Through Visual Tree Children of Child Dependency Object
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        //Return Child of Child
                        yield return childOfChild;
                    }
                }
            }
        }
        #endregion Get Elements
    }
}