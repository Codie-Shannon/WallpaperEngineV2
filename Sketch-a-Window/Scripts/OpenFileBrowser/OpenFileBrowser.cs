using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Sketch_a_Window.Scripts
{
    public class OpenFileBrowser
    {
        // Open File Picker
        // ======================================================================
        // ======================================================================
        public static async Task<StorageFile> OpenFilePickerAsync(string[] filetypes)
        {
            //Create New FileOpenPicker
            FileOpenPicker picker = new FileOpenPicker();

            //Set File Picker's View Mode
            picker.ViewMode = PickerViewMode.Thumbnail;

            //Set File Picker's Suggested Start Location
            picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;

            //Loop through fileTypes Array
            foreach (string type in filetypes)
            {
                //Add Current Looped File Type to File Picker's FileTypeFilter List
                picker.FileTypeFilter.Add($".{type}");
            }

            //Open File Picker and Create StorageFile from Selected Video
            StorageFile file = await picker.PickSingleFileAsync();

            //Validate File Selection
            if (file != null)
            {
                //Return StorageFile
                return file;
            }

            //Return Null
            return null;
        }
    }
}