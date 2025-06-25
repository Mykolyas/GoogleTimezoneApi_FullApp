using GoogleTimezoneSln.Models;
namespace GoogleTimezoneSln.Services;
using System.Globalization;
using System.Text.Json;


public class TimeZoneService : ITimeZoneService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public TimeZoneService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }
    public async Task<TimeZoneResponse> GetTimeZoneAsync(TimeZoneRequest request)
    {
        string url = $"https://maps.googleapis.com/maps/api/timezone/json?location={request.Latitude.ToString(CultureInfo.InvariantCulture)},{request.Longitude.ToString(CultureInfo.InvariantCulture)}&timestamp={request.Timestamp}&key={_apiKey}";
        var httpResponse = await _httpClient.GetAsync(url);
        httpResponse.EnsureSuccessStatusCode();//не обробляю його !!!

        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TimeZoneResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result!;
    }
}
