using GeoCodingApp.Models.Request;
using GeoCodingApp.Models.Response;

namespace GeoCodingApp.Services.Interfaces;

public interface IGeocodingService
{
    Task<List<GeocodeResponse>> GetCoordinatesAsync(GeocodeRequest request);
}