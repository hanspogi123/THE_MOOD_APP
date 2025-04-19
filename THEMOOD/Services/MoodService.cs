using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace THEMOOD.Services
{
    public interface IMoodService
    {
        List<MoodEntry> GetAllMoods();
        MoodEntry GetMoodForDay(DateTime date);
        void SaveMood(MoodEntry mood);
        void DeleteMood(DateTime date);
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

        public List<MoodEntry> GetAllMoods()
        {
            return _moods.OrderByDescending(m => m.Date).ToList();
        }

        public MoodEntry GetMoodForDay(DateTime date)
        {
            return _moods.FirstOrDefault(m => m.Date.Date == date.Date);
        }

        public void SaveMood(MoodEntry mood)
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

        public void DeleteMood(DateTime date)
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