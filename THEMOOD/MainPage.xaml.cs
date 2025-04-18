using THEMOOD.ViewModels;

namespace THEMOOD
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Connect the NavBar control with the ViewModel
            NavBar.SetViewModel(NavViewModel);
        }
    }
}