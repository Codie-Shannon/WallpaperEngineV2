using System;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using Sketch_a_Window.ViewModels;

namespace Sketch_a_Window.Pages
{
    public sealed partial class SettingsPage : Page
    {
        // Navigation View Model
        // ======================================================================
        // ======================================================================
        private NavigationViewModel vmNavigation;



        // Constructor
        // ======================================================================
        // ======================================================================
        public SettingsPage()
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
            //Create NavigationViewModel Object for vmNavigation Variable
            vmNavigation = new NavigationViewModel(nvNavigation, fFrame,
                new List<Tuple<string, NavigationViewItem, Page>>()
                {
                    new Tuple<string, NavigationViewItem, Page>("general", nviGeneral, new GeneralPage()),
                    new Tuple<string, NavigationViewItem, Page>("performance", nviPerformance, new PerformancePage()),
                    new Tuple<string, NavigationViewItem, Page>("about", nviAbout, new AboutPage())
                }
            );

            //Load Default Page
            vmNavigation.NavigateToPage("general", true);
        }



        // Event Handlers
        // ======================================================================
        // ======================================================================
        private void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Navigate to Page
            vmNavigation.NavigateToPage(((NavigationViewItem)sender).Content);
        }
    }
}