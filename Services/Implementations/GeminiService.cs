using GuitarShop.Models;
using GuitarShop.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace GuitarShop.Services.Implementations
{
    public class GeminiService : IGeminiServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiService(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _apiKey = config["Gemini:ApiKey"] ?? throw new Exception("Gemini API key missing");
        }

        public async Task<string> ChatWithContextAsync(List<ChatMessage> history)
        {
            // ✅ Cập nhật model và version mới
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-preview-09-2025:generateContent?key={_apiKey}";

            var requestBody = new
            {
                contents = history.Select(m => new
                {
                    role = m.Role,
                    parts = new[] { new { text = m.Content } }
                })
            };

            var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var response = await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            var body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Gemini API error: {body}");

            using var doc = JsonDocument.Parse(body);
            return doc.RootElement
                      .GetProperty("candidates")[0]
                      .GetProperty("content")
                      .GetProperty("parts")[0]
                      .GetProperty("text")
                      .GetString()!;
        }
    }
}
