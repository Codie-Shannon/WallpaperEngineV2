using Windows.Storage;

namespace Sketch_a_Window.Scripts
{
    public class LocalSettings
    {
        // Validate Value
        // ======================================================================
        // ======================================================================
        public static bool ValidateValue(string name)
        {
            //Check if the Value Exists within the ApplicationData's Local Settings
            if (ApplicationData.Current.LocalSettings.Values.Keys.Contains(name))
            {
                //Return True
                return true;
            }

            //Return False
            return false;
        }


        // Get Value
        // ======================================================================
        // ======================================================================
        public static object GetValue(string name)
        {
            //Return Value
            return ApplicationData.Current.LocalSettings.Values[name];
        }


        // Set Value
        // ======================================================================
        // ======================================================================
        public static void SetValue(string name, object value)
        {
            //Set Value
            ApplicationData.Current.LocalSettings.Values[name] = value;
        }
    }
}