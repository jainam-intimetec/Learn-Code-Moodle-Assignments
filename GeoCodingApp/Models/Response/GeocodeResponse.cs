namespace GeoCodingApp.Models.Response;

public class GeocodeResponse
{
    public string FormattedAddress { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}