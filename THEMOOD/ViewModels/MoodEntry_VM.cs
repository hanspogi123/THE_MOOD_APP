using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace THEMOOD.ViewModels
{
    public partial class MoodEntry_VM : ObservableObject
    {
        [ObservableProperty]
        private DateOnly date;

        [ObservableProperty]
        private string mood;

        [ObservableProperty]
        private string note;

        [ObservableProperty]
        private string moodIcon;

        // New property to return the formatted date string
        public string FormattedDate
        {
            get
            {
                // Convert DateOnly to DateTime to access day of week
                DateTime dateTime = date.ToDateTime(TimeOnly.MinValue);
                return dateTime.ToString("MMMM d, yyyy dddd");
            }
        }

    }
}