using Microsoft.Maui.Networking;

namespace THEMOOD.Services
{
    public class ConnectivityService
    {
        private static ConnectivityService _instance;
        public static ConnectivityService Instance => _instance ??= new ConnectivityService();

        private bool _isAlertShowing;

        private ConnectivityService()
        {
            // Check initial connection
            CheckConnection();

            // Subscribe to connectivity changes
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                ShowNoConnectionAlert();
            }
        }

        private void CheckConnection()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                ShowNoConnectionAlert();
            }
        }

        private async void ShowNoConnectionAlert()
        {
            if (_isAlertShowing) return;

            _isAlertShowing = true;
            while (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "No Internet Connection",
                        "Please check your internet connection and try again.",
                        "OK"
                    );
                });
            }
            _isAlertShowing = false;
        }
    }
} 