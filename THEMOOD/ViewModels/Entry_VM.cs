using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace THEMOOD.ViewModels
{

    public partial class Entry_VM : ObservableObject
    {
        [ObservableProperty]
        private string? email;

        [ObservableProperty]
        private string? password;

        [ObservableProperty]
        private string? name;

        public Entry_VM()
        {
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
            {
                await Shell.Current.DisplayAlert("Login", $"Welcome {Email}!", "OK");

                // Navigate to main page after successful login
                await Shell.Current.GoToAsync("main");

            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Email and Password are required.", "OK");
            }
        }


        [RelayCommand]
        private async Task SignUpLipatAsync()
        {
            await Shell.Current.GoToAsync("signup");
        }

        [RelayCommand]
        private async Task LoginLipatAsync()
        {
            await Shell.Current.GoToAsync("//login");
        }
    }
}

