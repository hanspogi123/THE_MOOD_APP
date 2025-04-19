using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using THEMOOD.Services;

namespace THEMOOD.ViewModels
{
    public partial class CalendarView_VM : ObservableObject
    {
        private readonly IMoodService _moodService;

        public CalendarView_VM(IMoodService moodService)
        {
            _moodService = moodService;
            LoadMonth(CurrentMonth);
        }

        // For design-time support
        public CalendarView_VM()
        {
            _moodService = new DefaultMoodService();
            LoadMonth(CurrentMonth);
        }

        [ObservableProperty]
        private DateTime currentMonth = DateTime.Today;

        [ObservableProperty]
        private ObservableCollection<DayMood> daysInMonth = new();

        [ObservableProperty]
        private string selectedFilter = "All";

        public ObservableCollection<string> MoodFilters { get; } = new()
        {
            "All", "Happy", "Sad", "Anxious", "Angry", "Excited",
            "Lonely", "Tired", "Motivated", "Stressed", "Bored", "Okay"
        };

        [RelayCommand]
        private void NextMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
            LoadMonth(CurrentMonth);
        }

        [RelayCommand]
        private void PreviousMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
            LoadMonth(CurrentMonth);
        }

        [RelayCommand]
        private async Task ViewDayDetails(DayMood day)
        {
            if (day == null || !day.HasMood)
                return;

            await Application.Current.MainPage.DisplayAlert(
                $"Mood on {day.Date.ToString("MMM dd")}",
                $"Mood: {day.Mood}\nReason: {day.Reason}",
                "Close");
        }

        [RelayCommand]
        private async Task AddMoodForDay(DayMood day)
        {
            if (day == null)
                return;

            // Navigate to mood entry page with the selected date
            var parameters = new Dictionary<string, object>
            {
                { "SelectedDate", day.Date }
            };

            await Shell.Current.GoToAsync("//MoodEntryPage", parameters);
        }

        [RelayCommand]
        private void FilterMoods()
        {
            LoadMonth(CurrentMonth);
        }

        private void LoadMonth(DateTime month)
        {
            DaysInMonth.Clear();

            // Get first day of the month
            var firstDayOfMonth = new DateTime(month.Year, month.Month, 1);

            // Get the number of days in the month
            var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);

            // Add empty slots for days before the first day of the month
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            for (int i = 0; i < firstDayOfWeek; i++)
            {
                DaysInMonth.Add(new DayMood { IsEmpty = true });
            }

            // Add days of the month
            for (int day = 1; day <= daysInMonth; day++)
            {
                var currentDate = new DateTime(month.Year, month.Month, day);
                var dayMood = GetMoodForDay(currentDate);

                if (SelectedFilter == "All" || dayMood.Mood == SelectedFilter || !dayMood.HasMood)
                {
                    DaysInMonth.Add(dayMood);
                }
                else
                {
                    // Add placeholder for filtered-out days
                    var placeholder = new DayMood
                    {
                        Date = currentDate,
                        IsFiltered = true
                    };
                    DaysInMonth.Add(placeholder);
                }
            }
        }

        private DayMood GetMoodForDay(DateTime date)
        {
            var moodEntry = _moodService.GetMoodForDay(date);

            if (moodEntry != null)
            {
                return new DayMood
                {
                    Date = moodEntry.Date,
                    Mood = moodEntry.Mood,
                    Reason = moodEntry.Reason,
                    HasMood = true
                };
            }

            return new DayMood { Date = date, HasMood = false };
        }
    }

    public class DayMood : ObservableObject
    {
        public DateTime Date { get; set; }
        public string Mood { get; set; }
        public string Reason { get; set; }
        public bool HasMood { get; set; }
        public bool IsEmpty { get; set; }
        public bool IsFiltered { get; set; }

        public string MoodColor
        {
            get
            {
                if (!HasMood)
                    return "#EEEEEE";

                return Mood switch
                {
                    "Happy" => "#FFD700", // Gold
                    "Sad" => "#6495ED",   // Blue
                    "Anxious" => "#FF7F50", // Coral
                    "Angry" => "#DC143C", // Crimson
                    "Excited" => "#FF4500", // OrangeRed
                    "Lonely" => "#9370DB", // Purple
                    "Tired" => "#708090",  // SlateGray
                    "Motivated" => "#32CD32", // LimeGreen
                    "Stressed" => "#FF6347", // Tomato
                    "Bored" => "#D3D3D3",   // LightGray
                    "Okay" => "#90EE90",    // LightGreen
                    _ => "#EEEEEE",         // Default gray
                };
            }
        }

        public string MoodIcon
        {
            get
            {
                if (!HasMood)
                    return "😶";

                return Mood switch
                {
                    "Happy" => "😊",
                    "Sad" => "😢",
                    "Anxious" => "😰",
                    "Angry" => "😠",
                    "Excited" => "😃",
                    "Lonely" => "😔",
                    "Tired" => "😴",
                    "Motivated" => "💪",
                    "Stressed" => "😫",
                    "Bored" => "😐",
                    "Okay" => "🙂",
                    _ => "😶",
                };
            }
        }
    }
}