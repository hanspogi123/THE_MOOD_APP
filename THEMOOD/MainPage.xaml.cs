using THEMOOD.ViewModels;

namespace THEMOOD
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Connect the NavBar control with the singleton ViewModel
            NavBar.SetViewModel(NavBarViewModel.Instance);
            Shell.SetNavBarIsVisible(this, false);
        }
    }
}