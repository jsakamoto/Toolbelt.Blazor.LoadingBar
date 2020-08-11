using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SampleSite.Shared;

namespace SampleSite.Client.Service
{
    public class WeatherForecastService
    {
        private readonly HttpClient _HttpClient;

        public WeatherForecastService(HttpClient httpClient)
        {
            _HttpClient = httpClient;
        }

        public async Task<WeatherForecast[]> FetchForecastsAsync(bool wait = false)
        {
            return await _HttpClient.GetFromJsonAsync<WeatherForecast[]>($"WeatherForecast?wait={wait}");
        }

        public async Task<WeatherForecast[]> FetchForecastsByPostAsync(bool wait = false)
        {
            var response = await _HttpClient.PostAsync($"WeatherForecast?wait={wait}", new StringContent(""));
            return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<WeatherForecast[]>();
        }
    }
}
