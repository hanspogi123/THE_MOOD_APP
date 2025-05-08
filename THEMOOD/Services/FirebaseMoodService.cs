using Firebase.Database;
using Firebase.Database.Query;
using System.Text.Json;

namespace THEMOOD.Services
{
    public class FirebaseMoodService : IMoodService
    {
        private readonly FirebaseClient _firebaseClient;
        private readonly string _userId;
        private const string FirebaseUrl = "https://themood-165fc-default-rtdb.asia-southeast1.firebasedatabase.app/";

        public FirebaseMoodService(string userId)
        {
            _userId = userId;
            Console.WriteLine($"Initializing FirebaseMoodService with userId: {userId}");
            _firebaseClient = new FirebaseClient(FirebaseUrl);
        }

        public async Task<List<MoodEntry>> GetAllMoods()
        {
            try
            {
                var moods = await _firebaseClient
                    .Child("users")
                    .Child(_userId)
                    .Child("moods")
                    .OnceAsync<MoodEntry>();

                return moods.Select(x => x.Object).OrderByDescending(m => m.Date).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting moods: {ex.Message}");
                return new List<MoodEntry>();
            }
        }

        public async Task<MoodEntry> GetMoodForDay(DateTime date)
        {
            try
            {
                var moods = await _firebaseClient
                    .Child("users")
                    .Child(_userId)
                    .Child("moods")
                    .OnceAsync<MoodEntry>();

                return moods.Select(x => x.Object)
                    .FirstOrDefault(m => m.Date.Date == date.Date);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting mood for day: {ex.Message}");
                return null;
            }
        }

        public async Task SaveMood(MoodEntry mood)
        {
            try
            {
                // Generate a unique key for the mood entry
                var moodKey = $"{mood.Date:yyyyMMdd}";

                // Save to Firebase
                await _firebaseClient
                    .Child("users")
                    .Child(_userId)
                    .Child("moods")
                    .Child(moodKey)
                    .PutAsync(mood);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving mood: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteMood(DateTime date)
        {
            try
            {
                var moodKey = $"{date:yyyyMMdd}";
                await _firebaseClient
                    .Child("users")
                    .Child(_userId)
                    .Child("moods")
                    .Child(moodKey)
                    .DeleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting mood: {ex.Message}");
                throw;
            }
        }
    }
} 