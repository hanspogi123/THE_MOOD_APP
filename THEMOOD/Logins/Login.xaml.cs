using Firebase.Auth;
using THEMOOD.Services;

namespace THEMOOD.Logins;


public partial class Login : ContentPage
{
    private readonly FirebaseAuthService _authService;
    public Login()
    {
        InitializeComponent();
        _authService = new FirebaseAuthService();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            var auth = await _authService.SignIn(Email.Text, Password.Text);
            string token = await _authService.GetFreshToken(auth);


            await DisplayAlert("Success", $"Welcome {auth.User.Email}\nToken: {token}", "OK");

            await Shell.Current.GoToAsync("//main");

        }
        catch (Exception ex)
        {
            await DisplayAlert("Login Failed", ex.Message, "OK");
        }
    }

    private async void SignUp_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//signup");
    }
}