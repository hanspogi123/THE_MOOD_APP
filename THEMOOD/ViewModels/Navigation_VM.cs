using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace THEMOOD.ViewModels
{
    public partial class NavBarViewModel : ObservableObject
    {
        public static Action<Microsoft.Maui.Controls.View>? SetMainPageContent;

        // Cache dictionary to store created views
        private Dictionary<string, Microsoft.Maui.Controls.View> _viewCache = new Dictionary<string, Microsoft.Maui.Controls.View>();

        private static NavBarViewModel _instance;
        public static NavBarViewModel Instance => _instance ??= new NavBarViewModel();

        // Make constructor private for singleton pattern
        private NavBarViewModel() { }

        private bool _isHomeActive = true;
        public bool IsHomeActive
        {
            get => _isHomeActive;
            set => SetProperty(ref _isHomeActive, value);
        }

        private bool _isWalletActive;
        public bool IsWalletActive
        {
            get => _isWalletActive;
            set => SetProperty(ref _isWalletActive, value);
        }

        private bool _isActivityActive;
        public bool IsActivityActive
        {
            get => _isActivityActive;
            set => SetProperty(ref _isActivityActive, value);
        }

        private bool _isProfileActive;
        public bool IsProfileActive
        {
            get => _isProfileActive;
            set => SetProperty(ref _isProfileActive, value);
        }

        [RelayCommand]
        private Task NavigateToHomeAsync()
        {
            IsHomeActive = true;
            IsWalletActive = false;
            IsActivityActive = false;
            IsProfileActive = false;

            // We navigate through shell for Home
            return Shell.Current.GoToAsync("//main");
        }

        [RelayCommand]
        private Task NavigateToWalletAsync()
        {
            IsHomeActive = false;
            IsWalletActive = true;
            IsActivityActive = false;
            IsProfileActive = false;

            return Shell.Current.GoToAsync("//main");
        }

        [RelayCommand]
        private Task NavigateToChatAsync()
        {
            IsHomeActive = false;
            IsWalletActive = false;
            IsActivityActive = false;
            IsProfileActive = false;

            // Use cached view if available, otherwise create a new one
            if (!_viewCache.ContainsKey("Chat"))
            {
                _viewCache["Chat"] = new THEMOOD.Pages.Chat();
            }

            SetMainPageContent?.Invoke(_viewCache["Chat"]);

            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task NavigateToActivityAsync()
        {
            IsHomeActive = false;
            IsWalletActive = false;
            IsActivityActive = true;
            IsProfileActive = false;

            // Use cached view if available, otherwise create a new one
            if (!_viewCache.ContainsKey("MoodEntry"))
            {
                _viewCache["MoodEntry"] = new THEMOOD.Pages.MoodEntryPage();
            }

            SetMainPageContent?.Invoke(_viewCache["MoodEntry"]);

            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task NavigateToProfileAsync()
        {
            IsHomeActive = false;
            IsWalletActive = false;
            IsActivityActive = false;
            IsProfileActive = true;

            // Use cached view if available, otherwise create a new one
            if (!_viewCache.ContainsKey("Meditation"))
            {
                _viewCache["Meditation"] = new THEMOOD.Pages.Meditation();
            }

            SetMainPageContent?.Invoke(_viewCache["Meditation"]);

            return Task.CompletedTask;
        }
    }
}