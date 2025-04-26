using Firebase.Auth;
using THEMOOD.Services;

namespace THEMOOD.Logins;

public partial class SignUp : ContentPage
{
	public SignUp()
	{
		InitializeComponent();
	}

    private async void LoginBalik_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//login");
    }
}