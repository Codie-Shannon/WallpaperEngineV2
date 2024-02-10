using System;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;

namespace Sketch_a_Window.Models
{
    public class NavigationModel
    {
        // Navigation Bar
        // ======================================================================
        // ======================================================================
        public NavigationView NavigationBar { get; set; }


        // Content
        // ======================================================================
        // ======================================================================
        public Frame Content { get; set; }


        // Pages
        // ======================================================================
        // ======================================================================
        public List<Tuple<string, NavigationViewItem, Page>> Pages { get; set; }
    }
}