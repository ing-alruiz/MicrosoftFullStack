using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace MyBlazorWasmApp
{
    public class AppSettingsService
    {
        private readonly HttpClient _httpClient;
        private JsonElement _settings;

        public AppSettingsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task LoadAsync()
        {
            var json = await _httpClient.GetStringAsync("appsettings.json");
            _settings = JsonSerializer.Deserialize<JsonElement>(json);
        }

        public JsonElement Settings => _settings;

        public JsonElement? GetSection(string section)
        {
            if (_settings.TryGetProperty(section, out var value))
                return value;
            return null;
        }
    }
}
