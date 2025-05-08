using THEMOOD.Services;

namespace THEMOOD.Pages;

public partial class Home : ContentView
{
	private readonly FirebaseAuthService _authService;

	public Home()
	{
		InitializeComponent();
		_authService = new FirebaseAuthService();
	}

	private async void OnLogoutClicked(object sender, EventArgs e)
	{
		try
		{
			// Clear mood entries
			MoodEntryService.Instance.Logout();
			
			// Sign out from Firebase
			await _authService.SignOut();
			
			// Navigate to login page
			await Shell.Current.GoToAsync("//login");
		}
		catch (Exception ex)
		{
			await Application.Current.MainPage.DisplayAlert("Error", $"Failed to log out: {ex.Message}", "OK");
		}
	}
}