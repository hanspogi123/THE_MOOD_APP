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
        // Using partial properties instead of [ObservableProperty] for AOT compatibility

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
            // Transfer doesn't have an active state
            
            IsHomeActive = false;
            IsWalletActive = false;
            IsActivityActive = false;
            IsProfileActive = false;

            SetMainPageContent?.Invoke(new THEMOOD.Pages.Chat());

            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task NavigateToActivityAsync()
        {
            IsHomeActive = false;
            IsWalletActive = false;
            IsActivityActive = true;
            IsProfileActive = false;

            SetMainPageContent?.Invoke(new THEMOOD.Pages.MoodEntryPage());

            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task NavigateToProfileAsync()
        {
            IsHomeActive = false;
            IsWalletActive = false;
            IsActivityActive = false;
            IsProfileActive = true;

            SetMainPageContent?.Invoke(new THEMOOD.Pages.Meditation());

            return Task.CompletedTask;
        }
    }
}