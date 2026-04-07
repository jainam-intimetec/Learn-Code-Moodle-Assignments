using GeoCodingApp.Models.Request;
using GeoCodingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeoCodingApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeocodingController : ControllerBase
{
    private readonly IGeocodingService _service;

    public GeocodingController(IGeocodingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string location)
    {
        try
        {
            var result = await _service.GetCoordinatesAsync(new GeocodeRequest
            {
                Location = location
            });

            return Ok(new
            {
                Success = true,
                Count = result.Count,
                Data = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
}