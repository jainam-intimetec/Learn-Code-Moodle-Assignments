using System.Text.Json;
using GeoCodingApp.Models.Request;
using GeoCodingApp.Models.Response;
using GeoCodingApp.Services.Interfaces;
using GeoCodingApp.Validators;

namespace GeoCodingApp.Services;

public class GeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;

    public GeocodingService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GeoCodingApp/1.0");
    }

    public async Task<List<GeocodeResponse>> GetCoordinatesAsync(GeocodeRequest request)
    {
        GeocodeRequestValidator.Validate(request);

        var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(request.Location)}&format=json";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new Exception("OSM API call failed");

        var json = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<List<OsmResponse>>(json);

        if (data == null || data.Count == 0)
            throw new Exception("No results found");

        return data.Select(d => new GeocodeResponse
        {
            FormattedAddress = d.display_name,
            Latitude = double.Parse(d.lat),
            Longitude = double.Parse(d.lon)
        }).ToList();
    }
}