using System.Diagnostics;
using THEMOOD.Logins;

namespace THEMOOD
{
    public partial class App : Application
    {
        public App()
        {
            try
            {
                InitializeComponent();
                MainPage = new AppShell(); // or your custom MainPage
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unhandled startup exception: " + ex);
                MainPage = new ContentPage { Content = new Label { Text = "Startup Error!" } };
            }
        }
    }
}
