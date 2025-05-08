using System.Text.Json;

namespace THEMOOD.Services
{
    public interface IMoodService
    {
        Task<List<MoodEntry>> GetAllMoods();
        Task<MoodEntry> GetMoodForDay(DateTime date);
        Task SaveMood(MoodEntry mood);
        Task DeleteMood(DateTime date);
    }

    public class MoodEntry
    {
        public DateTime Date { get; set; }
        public string Mood { get; set; }
        public string Reason { get; set; }
    }

    public class DefaultMoodService : IMoodService
    {
        private readonly string _filePath;
        private List<MoodEntry> _moods;

        public DefaultMoodService()
        {
            _filePath = Path.Combine(FileSystem.AppDataDirectory, "moods.json");
            _moods = LoadMoods();
        }

        public async Task<List<MoodEntry>> GetAllMoods()
        {
            return _moods.OrderByDescending(m => m.Date).ToList();
        }

        public async Task<MoodEntry> GetMoodForDay(DateTime date)
        {
            return _moods.FirstOrDefault(m => m.Date.Date == date.Date);
        }

        public async Task SaveMood(MoodEntry mood)
        {
            // Remove existing mood entry for this date if it exists
            var existing = _moods.FirstOrDefault(m => m.Date.Date == mood.Date.Date);
            if (existing != null)
            {
                _moods.Remove(existing);
            }

            // Add the new mood
            _moods.Add(mood);

            // Save to file
            SaveMoods();
        }

        public async Task DeleteMood(DateTime date)
        {
            var existing = _moods.FirstOrDefault(m => m.Date.Date == date.Date);
            if (existing != null)
            {
                _moods.Remove(existing);
                SaveMoods();
            }
        }

        private List<MoodEntry> LoadMoods()
        {
            if (!File.Exists(_filePath))
            {
                return new List<MoodEntry>();
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<MoodEntry>>(json) ?? new List<MoodEntry>();
            }
            catch
            {
                return new List<MoodEntry>();
            }
        }

        private void SaveMoods()
        {
            try
            {
                var json = JsonSerializer.Serialize(_moods);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving moods: {ex.Message}");
            }
        }
    }
}