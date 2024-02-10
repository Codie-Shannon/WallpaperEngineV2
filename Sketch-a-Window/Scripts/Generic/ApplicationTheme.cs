using Windows.Storage;
using Windows.UI.Xaml;

namespace Sketch_a_Window.Scripts
{
    // Source: https://stackoverflow.com/a/56918252
    class ApplicationTheme
    {
        #region Variables
        // Theme
        // ======================================================================
        // ======================================================================
        public const ElementTheme LightTheme = ElementTheme.Light;
        public const ElementTheme DarkTheme = ElementTheme.Dark;


        // Other
        // ======================================================================
        // ======================================================================
        private const string KeyTheme = "RequestedTheme";
        private static ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        #endregion Variables



        #region Properties
        // Theme Property
        // ======================================================================
        // ======================================================================
        public static ElementTheme Theme
        {
            get
            {
                //Validate Local Setting's RequestedTheme Variable
                if (LocalSettings.Values[KeyTheme] == null)
                {
                    //Set Local Setting's RequestedTheme Variable to Light Theme
                    LocalSettings.Values[KeyTheme] = (int)LightTheme;

                    //Return Light Theme
                    return LightTheme;
                }
                else if ((int)LocalSettings.Values[KeyTheme] == (int)LightTheme)
                {
                    //Return Light Theme
                    return LightTheme;
                }
                else
                {
                    //Return Dark Theme
                    return DarkTheme;
                }
            }
            set
            {
                //Validate Set Theme
                if (value == ElementTheme.Default)
                {
                    //Throw Exception
                    throw new System.Exception("Only set the theme to light or dark mode!");
                }
                else if ((int)value == (int)LocalSettings.Values[KeyTheme])
                {
                    //Return
                    return;
                }
                else
                {
                    //Set Local Setting's RequestedTheme Variable to Value
                    LocalSettings.Values[KeyTheme] = (int)value;
                }
            }
        }
        #endregion Properties
    }
}