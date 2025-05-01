using THEMOOD.ViewModels;
using System.Collections.ObjectModel;

namespace THEMOOD.Services
{
    public class MoodEntryService
    {
        private static MoodEntryService _instance;
        private IMoodService _moodService;
        private GeminiAIService _aiService;
        private readonly string _apiKey = "AIzaSyCOwo-lCI40SB7HRW6ad1xV28pxUIIoDjQ"; // Replace with your actual API key

        public static MoodEntryService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MoodEntryService();
                }
                return _instance;
            }
        }

        public ObservableCollection<MoodEntry_VM> MoodEntries { get; private set; }

        private MoodEntryService()
        {
            _moodService = new DefaultMoodService();
            _aiService = new GeminiAIService(_apiKey);
            LoadMoodEntries();
        }

        private void LoadMoodEntries()
        {
            var entries = _moodService.GetAllMoods();
            MoodEntries = new ObservableCollection<MoodEntry_VM>(
                entries.Select(e => new MoodEntry_VM
                {
                    Date = DateOnly.FromDateTime(e.Date),
                    Mood = e.Mood,
                    Note = e.Reason,
                    MoodIcon = GetMoodIcon(e.Mood)
                })
            );
        }

        public void AddMoodEntry(MoodEntry_VM entry)
        {
            entry.MoodIcon = GetMoodIcon(entry.Mood);
            // Create a MoodEntry from the view model
            var moodEntry = new MoodEntry
            {
                Date = entry.Date.ToDateTime(TimeOnly.MinValue),
                Mood = entry.Mood,
                Reason = entry.Note
            };

            // Save to the service
            _moodService.SaveMood(moodEntry);

            // Update the observable collection
            // First check if an entry for this date already exists
            var existingEntry = MoodEntries.FirstOrDefault(e => e.Date == entry.Date);

            if (existingEntry != null)
            {
                MoodEntries.Remove(existingEntry);
                Shell.Current.DisplayAlert("We have removed your previous mood entry on this date",
                $"\nDate: {moodEntry.Date.ToShortDateString()}", "OK");
            }

            // Add the new entry
            MoodEntries.Add(entry);
        }

        public void RemoveMoodEntry(MoodEntry_VM entry)
        {
            // Delete from the service
            _moodService.DeleteMood(entry.Date.ToDateTime(TimeOnly.MinValue));

            // Remove from the observable collection
            MoodEntries.Remove(entry);
        }

        // New method to get AI analysis of mood patterns
        public async Task<string> GetMoodAnalysis()
        {
            // Only analyze if we have at least 3 entries
            if (MoodEntries.Count < 3)
            {
                return "Enter at least 3 mood entries to receive AI analysis.";
            }

            // Convert view models to MoodEntry objects for the AI service
            var moodEntries = MoodEntries.Select(vm => new MoodEntry
            {
                Date = vm.Date.ToDateTime(TimeOnly.MinValue),
                Mood = vm.Mood,
                Reason = vm.Note
            }).ToList();

            // Get analysis from the AI service
            return await _aiService.AnalyzeMoodPatterns(moodEntries);
        }

        private string GetMoodIcon(string mood)
        {
            // Convert mood to emoji based on the mood text
            switch (mood?.ToLower())
            {
                case "angry": return "😠";
                case "anxious": return "😰";
                case "bored": return "😐";
                case "calm": return "😌";
                case "content": return "😊";
                case "depressed": return "😞";
                case "envious": return "😒";
                case "grateful": return "🙏";
                case "guilty": return "😔";
                case "happy": return "😄";
                case "hopeful": return "🌈";
                case "irritated": return "😤";
                case "lonely": return "😢";
                case "loving": return "❤️";
                case "neutral": return "😶";
                case "optimistic": return "🤞";
                case "pleased": return "😁";
                case "sad": return "😢";
                case "stressed": return "😫";
                default: return "❓";
            }
        }
    }
}