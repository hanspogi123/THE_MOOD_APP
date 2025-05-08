using System.Text.Json;
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
        _ = ConnectivityService.Instance;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            var auth = await _authService.SignIn(Email.Text, Password.Text);
            string token = await _authService.GetFreshToken(auth);
            
            // Initialize MoodEntryService with user ID
            MoodEntryService.Instance.Initialize(auth.User.LocalId);
            
            await DisplayAlert("Success", $"Welcome {auth.User.Email}", "OK");
            await Shell.Current.GoToAsync("//main");
        }
        catch (FirebaseAuthException authEx)
        {
            // Parse the error response from Firebase
            string reason = ExtractReasonFromFirebaseError(authEx.ResponseData);
            await DisplayAlert("Login Failed", $"{reason}", "OK");
        }
        catch (Exception ex)
        {
            // Generic error handling for non-Firebase exceptions
            await DisplayAlert("Login Failed", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void SignUp_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//signup");
    }

    private string ExtractReasonFromFirebaseError(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return "No response data available";
        }

        try
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Check for the Firebase error structure
            if (root.TryGetProperty("error", out JsonElement errorElement))
            {
                // First, try to get the message directly if available
                if (errorElement.TryGetProperty("message", out JsonElement messageElement))
                {
                    return messageElement.GetString();
                }

                // Otherwise, look for the first error's reason
                if (errorElement.TryGetProperty("errors", out JsonElement errorsArray) &&
                    errorsArray.ValueKind == JsonValueKind.Array &&
                    errorsArray.GetArrayLength() > 0)
                {
                    var firstError = errorsArray[0];
                    if (firstError.TryGetProperty("reason", out JsonElement reasonElement))
                    {
                        return reasonElement.GetString();
                    }
                    else if (firstError.TryGetProperty("message", out JsonElement errorMessageElement))
                    {
                        return errorMessageElement.GetString();
                    }
                }
            }

            // Handle the possibility of a different structure
            if (root.TryGetProperty("message", out JsonElement rootMessageElement))
            {
                return rootMessageElement.GetString();
            }
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"Error parsing JSON response: {jsonEx.Message}");
            return $"Invalid response format: {jsonEx.Message}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing response: {ex.Message}");
        }

        return "Unknown error";
    }
}