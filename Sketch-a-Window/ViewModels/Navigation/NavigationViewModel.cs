using System;
using Sketch_a_Window.Models;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;

namespace Sketch_a_Window.ViewModels
{
    public class NavigationViewModel : NavigationModel
    {
        // Constructor
        // ======================================================================
        // ======================================================================
        public NavigationViewModel(NavigationView navigation, Frame frame, List<Tuple<string, NavigationViewItem, Page>> pages)
        {
            //Set Variables
            NavigationBar = navigation;
            Content = frame;
            Pages = pages;
        }



        // Methods
        // ======================================================================
        // ======================================================================
        public void NavigateToPage(object content, bool isDefault = false)
        {
            //Get Page Name
            string name = content.ToString().ToLower();

            //Get Page From Pages List
            Tuple<string, NavigationViewItem, Page> element = GetPage(name);

            //Check if the Default Page is Being Set. If So, Set Navigation Bar's Selected Item
            if (isDefault) { NavigationBar.SelectedItem = element.Item2; }

            //Navigate to Page
            Content.Navigate(element.Item3.GetType());
        }



        // Extensions
        // ======================================================================
        // ======================================================================
        private Tuple<string, NavigationViewItem, Page> GetPage(string name)
        {
            //Loop through Pages List
            foreach (Tuple<string, NavigationViewItem, Page> tuple in Pages)
            {
                //Check if the Current Looped Tuple Contains the Page to Load
                if (tuple.Item1 == name)
                {
                    //Return Current Looped Tuple
                    return tuple;
                }
            }

            //Return First Tuple in List
            return Pages[0];
        }
    }
}