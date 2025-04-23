using THEMOOD.ViewModels;
using System.Collections.ObjectModel;

namespace THEMOOD.Services
{
    public class MoodEntryService
    {
        private static MoodEntryService _instance;
        private IMoodService _moodService;

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