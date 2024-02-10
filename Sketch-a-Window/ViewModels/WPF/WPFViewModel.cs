using System;
using Windows.UI.Xaml;
using Sketch_a_Window.Models;
using Sketch_a_Window.Scripts;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;

namespace Sketch_a_Window.ViewModels
{
    public class WPFViewModel : WPFModel
    {
        // Event Handlers
        // ======================================================================
        // ======================================================================
        private void App_Suspending(object sender, SuspendingEventArgs e)
        {
            //Set Local Setting's Closed Value to True
            LocalSettings.SetValue("Closed", true);
        }



        // Creation
        // ======================================================================
        // ======================================================================
        public async void Create()
        {
            //Validate Creation and Full Trust App Contract
            if (ValidateCreation() && ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                //Launch Full Trust Process For Current Application
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();

                //Set isApplicationActive to True
                isApplicationActive = true;
            }
        }



        // Validation
        // ======================================================================
        // ======================================================================
        private bool ValidateCreation()
        {
            //Check if a WPF Window has Already Been Created
            if (isApplicationActive == false)
            {
                //Set Suspending Event Handler
                Application.Current.Suspending += new SuspendingEventHandler(App_Suspending);

                //Set Local Setting's Closed Value to False
                LocalSettings.SetValue("Closed", false);

                //Return True
                return true;
            }

            //Return False
            return false;
        }
    }
}