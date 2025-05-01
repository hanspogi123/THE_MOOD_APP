using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace THEMOOD.Services
{
    public class GeminiAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

        public GeminiAIService(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<string> AnalyzeMoodPatterns(List<MoodEntry> moodEntries)
        {
            if (moodEntries == null || moodEntries.Count < 3)
            {
                return "Need at least 3 mood entries for analysis.";
            }

            try
            {
                // Prepare the data about mood entries
                var moodData = new StringBuilder();
                moodData.AppendLine("Recent mood entries:");

                foreach (var entry in moodEntries.OrderByDescending(e => e.Date).Take(10))
                {
                    moodData.AppendLine($"Date: {entry.Date.ToString("yyyy-MM-dd")}, Mood: {entry.Mood}, Reason: {entry.Reason}");
                }

                // Create the prompt for the AI
                string prompt = $@"
                You are a helpful mood analysis assistant. Based on the following mood entries, provide a brief, supportive analysis 
                of the person's mood patterns. Offer gentle observations and maybe one small suggestion if appropriate. 
                Keep the response short (3-4 sentences maximum) and positive.
                
                {moodData}
                ";

                // Prepare the request
                var requestContent = new GeminiRequest
                {
                    Contents = new List<GeminiContent>
                    {
                        new GeminiContent
                        {
                            Parts = new List<GeminiPart>
                            {
                                new GeminiPart { Text = prompt }
                            }
                        }
                    }
                };

                string jsonRequest = JsonSerializer.Serialize(requestContent);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Send the request
                var response = await _httpClient.PostAsync($"{_baseUrl}?key={_apiKey}", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(jsonResponse);

                    if (geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text != null)
                    {
                        return geminiResponse.Candidates[0].Content.Parts[0].Text;
                    }
                    else
                    {
                        return "Could not interpret the AI response.";
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {errorContent}");
                    return "Error getting mood analysis.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in mood analysis: {ex.Message}");
                return $"Could not complete mood analysis at this time.";
            }
        }
    }

    // Classes for Gemini API serialization
    public class GeminiRequest
    {
        [JsonPropertyName("contents")]
        public List<GeminiContent> Contents { get; set; }
    }

    public class GeminiContent
    {
        [JsonPropertyName("parts")]
        public List<GeminiPart> Parts { get; set; }
    }

    public class GeminiPart
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public List<GeminiCandidate> Candidates { get; set; }
    }

    public class GeminiCandidate
    {
        [JsonPropertyName("content")]
        public GeminiContent Content { get; set; }
    }
}