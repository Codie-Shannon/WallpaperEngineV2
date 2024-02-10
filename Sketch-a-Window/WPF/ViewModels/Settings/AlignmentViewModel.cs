using System;
using WPF.Models;
using System.Windows;
using System.Windows.Controls;

namespace WPF.ViewModels
{
    public class AlignmentViewModel : AlignmentModel
    {
        // Constructor
        // ======================================================================
        // ======================================================================
        public AlignmentViewModel(WallpaperViewModel model, MediaElement mewallpaper)
        {
            //Set Model
            vmWallpaper = model;

            //Set Content
            Content = mewallpaper;
        }



        #region Methods
        // Content
        // ======================================================================
        // ======================================================================
        public void SetContent(MediaElement content)
        {
            //Set Content
            Content = content;
        }


        // Alignment
        // ======================================================================
        // ======================================================================
        public void Alignment(string alignment)
        {
            //Get Alignment Type
            AlignmentType type = GetAlignmentType(alignment);

            //Set Alignment
            SetAlignment(type);
        }


        // Set Alignment
        // ======================================================================
        // ======================================================================
        private void SetAlignment(AlignmentType type)
        {
            //Get Alignment Object
            Tuple<AlignmentType, VerticalAlignment, HorizontalAlignment> alignmentObject = GetAlignmentObject(type);

            //Set Alignment
            Content.VerticalAlignment = alignmentObject.Item2;
            Content.HorizontalAlignment = alignmentObject.Item3;
        }
        #endregion Methods



        #region Extensions
        // Get Alignment Type
        // ======================================================================
        // ======================================================================
        private AlignmentType GetAlignmentType(string alignment)
        {
            //Validate and Get Alignment Type
            if (alignment == "left") { return AlignmentType.Left; }
            else if (alignment == "right") { return AlignmentType.Right; }
            else if (alignment == "center") { return AlignmentType.Center; }
            else { return AlignmentType.Stretch; }
        }


        // Get Alignment Object
        // ======================================================================
        // ======================================================================
        private Tuple<AlignmentType, VerticalAlignment, HorizontalAlignment> GetAlignmentObject(AlignmentType type)
        {
            //Loop through Items in AlignmentObjects List
            foreach (Tuple<AlignmentType, VerticalAlignment, HorizontalAlignment> item in AlignmentObjects)
            {
                //Check if the Current Looped Tuple's AlignmentType Matches the type Variable
                if (item.Item1 == type)
                {
                    //Return the Current Looped Tuple
                    return item;
                }
            }

            //Return Default Alignment Object
            return AlignmentObjects[0];
        }
        #endregion Extensions
    }
}