using GeoCodingApp.Models.Request;

namespace GeoCodingApp.Validators;

public static class GeocodeRequestValidator
{
    public static void Validate(GeocodeRequest request)
    {
        if (request == null)
            throw new ArgumentException("Request cannot be null");

        if (string.IsNullOrWhiteSpace(request.Location))
            throw new ArgumentException("Location is required");
    }
}