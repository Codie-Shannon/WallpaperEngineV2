using System;
using System.Windows;
using WPF.ViewModels;
using System.Windows.Controls;
using System.Collections.Generic;

namespace WPF.Models
{
    public class AlignmentModel
    {
        // Content
        // ======================================================================
        // ======================================================================
        public MediaElement Content { get; set; }


        // Wallpaper View Model
        // ======================================================================
        // ======================================================================
        public WallpaperViewModel vmWallpaper { get; set; }


        // Alignment Type
        // ======================================================================
        // ======================================================================
        public enum AlignmentType { Stretch, Left, Right, Center }


        // Alignments
        // ======================================================================
        // ======================================================================
        public List<Tuple<AlignmentType, VerticalAlignment, HorizontalAlignment>> AlignmentObjects { get; } = new List<Tuple<AlignmentType, VerticalAlignment, HorizontalAlignment>>()
        {
            { new Tuple<AlignmentType, VerticalAlignment, HorizontalAlignment>(AlignmentType.Stretch, VerticalAlignment.Stretch, HorizontalAlignment.Stretch) },
            { new Tuple<AlignmentType, VerticalAlignment, HorizontalAlignment>(AlignmentType.Left, VerticalAlignment.Center, HorizontalAlignment.Left) },
            { new Tuple<AlignmentType, VerticalAlignment, HorizontalAlignment>(AlignmentType.Right, VerticalAlignment.Center, HorizontalAlignment.Right) },
            { new Tuple<AlignmentType, VerticalAlignment, HorizontalAlignment>(AlignmentType.Center, VerticalAlignment.Center, HorizontalAlignment.Center) }
        };
    }
}