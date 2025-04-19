using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace THEMOOD.ViewModels
{
    public partial class CalendarView_VM:ObservableObject
    {
        [ObservableProperty]
        private DateTime selectedDate = DateTime.Today;

        [ObservableProperty]
        private ObservableCollection<DayMood> daysInMonth = new();

        [ObservableProperty]
        private string selectedFilter = "All";

        public ObservableCollection<string> Filters { get; } = new()
        {
            "All", "Happy", "Sad", "Anxious", "Angry", "Excited",
            "Lonely", "Tired", "Motivated", "Stressed", "Bored"
        };

        public CalendarView_VM()
        {
            LoadMonthCurrentMonth);
        }

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
                $"Mood : {day.Mood}\nReason: {day.Reason}",
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

            await Shell.Current.GoToAsync("moodentry", parameters);
        }

        [RelayCommand]
        private void FilterMoods(string filter)
        {
            LoadMonth(CurrentMonth);
        }

        private void LoadMonth(DateTime month)
        {
            DaysInMonth.clear();

            // Get first day of the month
            var firstDayOfMonth = new DateTime(month.Year, month.Month, 1);

            // Get the number of days in the month
            var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);

            // Add empty slots for days before the first day of the month
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            // Add days of the month
            for (int i = 0; i < firstDayOfWeek; i++)
            {
                DaysInMonth.Add(new DayMood { IsEmpty = true });
            }

            for (int i = 1; i <= daysInMonth; i++)
            {
                var currentDate = new DateTime(month.Year, month.Month, day);
                var dayMood = GetMoodForDay(currentDate);

                if(SelectedFilter == "All" || dayMood.Mood == SelectedFilter || !dayMood.HasMood)
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
                    DaysInMonth.add(placeholder);
                }
            }
        }

        private DayMood GetMoodForDay(DateTime date)
        {

            // This would normally fetch from a database or storage service
            // For demo purposes, we'll simulate some saved moods

            // Example data - replace with your actual data access logic
            var savedMoods = new List<DayMood>
            {
                new DayMood
                {
                    Date = DateTime.Today.AddDays(-2),
                    Mood = "Happy",
                    Reason = "Had a great day at work!",
                    HasMood = true
                },
                new DayMood
                {
                    Date = DateTime.Today.AddDays(-1),
                    Mood = "Sad",
                    Reason = "Missed an important event.",
                    HasMood = true
                },
                new DayMood
                {
                    Date = DateTime.Today,
                    Mood = "Excited",
                    Reason = "Going on a trip tomorrow!",
                    HasMood = true
                }
            };

            var mood = savedMoods.FirstOrDefault(m => m.Date.Date == date.Date);

            return mood ?? new DayMood { Date = date, HasMood = false };
        }
    }

    public class DayMood:ObservableObject
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
                if(!HasMood)
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
                if(!HasMood)
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
