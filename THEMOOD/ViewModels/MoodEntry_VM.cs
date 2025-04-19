using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;

namespace THEMOOD.ViewModels
{
    [QueryProperty(nameof(SelectedDate), "SelectedDate")]
    public partial class MoodEntry_VM : ObservableObject
    {
        private IMoodService _moodService;

        public MoodEntry_VM(IMoodService moodService)
        {
            _moodService = moodService;
        }

        // For design-time support
        public MoodEntry_VM()
        {
            _moodService = new DefaultMoodService();
        }

        [ObservableProperty]
        DateTime selectedDate = DateTime.Today;

        [ObservableProperty]
        string selectedMood;

        [ObservableProperty]
        string moodReason;

        [ObservableProperty]
        bool isEditing = false;

        public ObservableCollection<string> Moods { get; } = new()
        {
            "Happy", "Sad", "Anxious", "Angry", "Excited",
            "Lonely", "Tired", "Motivated", "Stressed", "Bored", "Okay"
        };

        partial void OnSelectedDateChanged(DateTime value)
        {
            LoadExistingMood();
        }

        private void LoadExistingMood()
        {
            var existingMood = _moodService.GetMoodForDay(SelectedDate);

            if (existingMood != null)
            {
                SelectedMood = existingMood.Mood;
                MoodReason = existingMood.Reason;
                IsEditing = true;
            }
            else
            {
                SelectedMood = null;
                MoodReason = string.Empty;
                IsEditing = false;
            }
        }

        [RelayCommand]
        async Task Submit()
        {
            if (string.IsNullOrWhiteSpace(SelectedMood))
            {
                await Application.Current.MainPage.DisplayAlert("Missing Info", "Please select your mood.", "OK");
                return;
            }

            // Save mood
            _moodService.SaveMood(new MoodEntry
            {
                Date = SelectedDate,
                Mood = SelectedMood,
                Reason = MoodReason
            });

            string message = IsEditing ? "Mood Updated" : "Mood Recorded";
            await Application.Current.MainPage.DisplayAlert(message, $"Date: {SelectedDate:MMM dd}\nMood: {SelectedMood}\nReason: {MoodReason}", "OK");

            // Navigate back
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        async Task Delete()
        {
            if (!IsEditing)
                return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Delete Mood",
                $"Are you sure you want to delete the mood entry for {SelectedDate:MMM dd}?",
                "Yes", "No");

            if (confirm)
            {
                _moodService.DeleteMood(SelectedDate);
                await Application.Current.MainPage.DisplayAlert("Deleted", "Mood entry was deleted", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        [RelayCommand]
        async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}