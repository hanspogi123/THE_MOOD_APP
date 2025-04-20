using THEMOOD.ViewModels;
using THEMOOD.Services;
using CommunityToolkit.Maui.Views;  

namespace THEMOOD;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        // Inject NavBar ViewModel singleton
        NavBar.SetViewModel(NavBarViewModel.Instance);

        PopupService.Initialize(
            popup => this.ShowPopup(popup),
            async popup => await this.ShowPopupAsync(popup)
        );

        // Set default content if you want
        MainContentArea.Content = new Label
        {
            Text = "Welcome to THEMOOD!",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        // Hook up dynamic content loader
        NavBarViewModel.SetMainPageContent = view =>
        {
            MainContentArea.Content = view;
        };
    }
}
