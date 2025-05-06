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
        private readonly List<GeminiContent> _conversationHistory;

        public ChatService(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
            _conversationHistory = new List<GeminiContent>();
        }

        public async Task<string> SendMessageAsync(string userMessage)
        {
            try
            {
                Console.WriteLine($"Sending message: {userMessage}");

                // Add user message to conversation history
                var userContent = new GeminiContent
                {
                    Role = "user",
                    Parts = new List<GeminiPart>
                    {
                        new GeminiPart { Text = userMessage }
                    }
                };
                _conversationHistory.Add(userContent);

                // Create request payload with conversation history
                var requestData = new GeminiRequest
                {
                    Contents = _conversationHistory
                };

                // Serialize to JSON
                var jsonContent = JsonSerializer.Serialize(requestData);
                Console.WriteLine($"Request JSON: {jsonContent}");
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Send request
                var response = await _httpClient.PostAsync($"{ApiUrl}?key={_apiKey}", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"API Error: {response.StatusCode} - {responseContent}");
                }

                // Parse response
                var responseObject = JsonSerializer.Deserialize<GeminiResponse>(responseContent);

                if (responseObject?.Candidates == null || !responseObject.Candidates.Any())
                {
                    throw new Exception("No response candidates received from API");
                }

                var aiResponse = responseObject.Candidates[0].Content?.Parts?.FirstOrDefault()?.Text
                    ?? "Sorry, I couldn't generate a response.";

                Console.WriteLine($"AI Response: {aiResponse}");

                // Add AI response to conversation history
                var aiContent = new GeminiContent
                {
                    Role = "model",
                    Parts = new List<GeminiPart>
                    {
                        new GeminiPart { Text = aiResponse }
                    }
                };
                _conversationHistory.Add(aiContent);

                // Keep only last 10 messages to prevent context window overflow
                if (_conversationHistory.Count > 10)
                {
                    _conversationHistory.RemoveRange(0, 2); // Remove oldest user-model pair
                }

                return aiResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendMessageAsync: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return $"Error: {ex.Message}";
            }
        }

        public void ClearConversation()
        {
            _conversationHistory.Clear();
        }
    }
}