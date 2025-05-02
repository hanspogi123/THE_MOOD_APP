using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace THEMOOD.Services
{
    /// <summary>
    /// Service for handling chat interactions with the Gemini API
    /// </summary>
    class ChatService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string ApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

        public ChatService(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<string> SendMessageAsync(string userMessage)
        {
            try
            {
                // Create request payload
                var requestData = new GeminiRequest
                {
                    Contents = new List<GeminiContent>
                    {
                        new GeminiContent
                        {
                            Parts = new List<GeminiPart>
                            {
                                new GeminiPart { Text = userMessage }
                            }
                        }
                    }
                };

                // Serialize to JSON
                var jsonContent = JsonSerializer.Serialize(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Send request
                var response = await _httpClient.PostAsync($"{ApiUrl}?key={_apiKey}", content);
                response.EnsureSuccessStatusCode();

                // Parse response
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<GeminiResponse>(responseContent);

                // Extract and return the AI's response text
                return responseObject?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text
                    ?? "Sorry, I couldn't generate a response.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }
    }
}