using Windows.UI.Xaml.Controls;

namespace Sketch_a_Window.Scripts
{
    public class Navigation
    {
        // Navigate to Page
        // ======================================================================
        // ======================================================================
        public static void NavigateToPage(Frame frame, Page page, NavigationView navigation, NavigationViewItem item)
        {
            //Navigate to Page
            frame.Navigate(page.GetType());

            //Set Selected Navigation View Item
            navigation.SelectedItem = item;
        }
    }
}