using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace THEMOOD.ViewModels
{
    public partial class NavBarViewModel : ObservableObject
    {
        public static Action<Microsoft.Maui.Controls.View>? SetMainPageContent;

        // Cache dictionary to store created views
        private Dictionary<string, Lazy<Microsoft.Maui.Controls.View>> _viewCache = new Dictionary<string, Lazy<Microsoft.Maui.Controls.View>>();

        // Track the current view to avoid unnecessary reloads
        private string _currentViewKey = string.Empty;

        private static NavBarViewModel _instance;
        public static NavBarViewModel Instance => _instance ??= new NavBarViewModel();

        // Make constructor private for singleton pattern
        private NavBarViewModel()
        {
            // Define lazy-loaded views
            _viewCache["Chat"] = new Lazy<Microsoft.Maui.Controls.View>(() => new THEMOOD.Pages.Chat());
            _viewCache["MoodEntry"] = new Lazy<Microsoft.Maui.Controls.View>(() => new THEMOOD.Pages.MoodEntryPage());
            _viewCache["Meditation"] = new Lazy<Microsoft.Maui.Controls.View>(() => new THEMOOD.Pages.Meditation());
            _viewCache["Voice"] = new Lazy<Microsoft.Maui.Controls.View>(() => new THEMOOD.Pages.Voice());
            _viewCache["Home"] = new Lazy<Microsoft.Maui.Controls.View>(() => new THEMOOD.Pages.Home());
        }

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

        private void ResetAllTabs()
        {
            IsHomeActive = false;
            IsWalletActive = false;
            IsActivityActive = false;
            IsProfileActive = false;
        }

        [RelayCommand]
        private Task NavigateToHomeAsync()
        {
            if (_currentViewKey == "Home")
                return Task.CompletedTask;

            ResetAllTabs();
            IsHomeActive = true;
            _currentViewKey = "Home";

            // Use lazy-loaded view - will create the first time it's accessed
            SetMainPageContent?.Invoke(_viewCache["Home"].Value);

            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task NavigateToWalletAsync()
        {
            if (_currentViewKey == "Voice")
                return Task.CompletedTask;

            ResetAllTabs();
            IsWalletActive = true;
            _currentViewKey = "Voice";

            // Use lazy-loaded view - will create the first time it's accessed
            SetMainPageContent?.Invoke(_viewCache["Voice"].Value);

            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task NavigateToChatAsync()
        {
            // Skip if already on this tab and view is set
            if (_currentViewKey == "Chat")
                return Task.CompletedTask;

            ResetAllTabs();
            _currentViewKey = "Chat";

            // Use lazy-loaded view - will create the first time it's accessed
            SetMainPageContent?.Invoke(_viewCache["Chat"].Value);

            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task NavigateToActivityAsync()
        {
            // Skip if already on this tab and view is set
            if (_currentViewKey == "MoodEntry")
                return Task.CompletedTask;

            ResetAllTabs();
            IsActivityActive = true;
            _currentViewKey = "MoodEntry";

            // Use lazy-loaded view - will create the first time it's accessed
            SetMainPageContent?.Invoke(_viewCache["MoodEntry"].Value);

            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task NavigateToProfileAsync()
        {
            // Skip if already on this tab and view is set
            if (_currentViewKey == "Meditation")
                return Task.CompletedTask;

            ResetAllTabs();
            IsProfileActive = true;
            _currentViewKey = "Meditation";

            var meditationView = _viewCache["Meditation"].Value as THEMOOD.Pages.Meditation;
            SetMainPageContent?.Invoke(meditationView);


            return Task.CompletedTask;
        }

        // Method to clear all cached views - useful for low memory situations
        public void ClearViewCache()
        {
            // Only clear non-active views
            var keysToRemove = _viewCache.Keys
                .Where(key => key != _currentViewKey)
                .ToList();

            foreach (var key in keysToRemove)
            {
                // Only remove if initialized
                if (_viewCache[key].IsValueCreated)
                {
                    _viewCache.Remove(key);
                }
            }

            // Add back lazy loaders for removed views
            if (!_viewCache.ContainsKey("Chat"))
                _viewCache["Chat"] = new Lazy<Microsoft.Maui.Controls.View>(() => new THEMOOD.Pages.Chat());

            if (!_viewCache.ContainsKey("MoodEntry"))
                _viewCache["MoodEntry"] = new Lazy<Microsoft.Maui.Controls.View>(() => new THEMOOD.Pages.MoodEntryPage());

            if (!_viewCache.ContainsKey("Meditation"))
                _viewCache["Meditation"] = new Lazy<Microsoft.Maui.Controls.View>(() => new THEMOOD.Pages.Meditation());

            if (!_viewCache.ContainsKey("Voice"))
                _viewCache["Voice"] = new Lazy<Microsoft.Maui.Controls.View>(() => new THEMOOD.Pages.Voice());

            if (!_viewCache.ContainsKey("Home"))
                _viewCache["Home"] = new Lazy<Microsoft.Maui.Controls.View>(() => new THEMOOD.Pages.Home());

            // Force garbage collection
            GC.Collect();
        }
    }
}