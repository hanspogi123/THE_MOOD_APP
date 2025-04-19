namespace THEMOOD.Pages;

public partial class MoodEntryPage : ContentPage
{
    public MoodEntryPage()
    {
        InitializeComponent();

        // Connect the NavBar control with the singleton ViewModel
        NavBar.SetViewModel(THEMOOD.ViewModels.NavBarViewModel.Instance);
        Shell.SetNavBarIsVisible(this, false);
    }
}