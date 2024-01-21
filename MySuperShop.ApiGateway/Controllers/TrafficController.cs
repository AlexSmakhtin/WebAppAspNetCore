using Microsoft.AspNetCore.Mvc;
using MySuperShop.ApiGateway.Services;

namespace MySuperShop.ApiGateway.Controllers;

[Route("api/traffic")]
[ApiController]
public class TrafficController : ControllerBase
{
    private readonly ILogger<TrafficController> _logger;
    private readonly TrafficMeasurementService _trafficMeasurementService;

    public TrafficController(ILogger<TrafficController> logger, TrafficMeasurementService trafficMeasurementService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _trafficMeasurementService = trafficMeasurementService ??
                                     throw new ArgumentNullException(nameof(trafficMeasurementService));
    }

    [HttpGet("get")]
    public Task<ActionResult<Dictionary<string, int>>> GetTraffic()
    {
        var trafficDict = _trafficMeasurementService.GetTraffic();
        return Task.FromResult<ActionResult<Dictionary<string, int>>>(trafficDict);
    }
}