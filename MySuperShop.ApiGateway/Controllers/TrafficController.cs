using Microsoft.AspNetCore.Mvc;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories.Interfaces;
using MySuperShop.Domain.Services;

namespace MySuperShop.ApiGateway.Controllers;

[Route("api/traffic")]
[ApiController]
public class TrafficController : ControllerBase
{
    private readonly ILogger<TrafficController> _logger;

    public TrafficController(ILogger<TrafficController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("get")]
    public async Task<ActionResult<List<TrafficInfo>>> GetTraffic(
        ITrafficMeasurementService trafficMeasurementService,
        ITrafficRepository trafficRepository,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(trafficMeasurementService);
        ArgumentNullException.ThrowIfNull(trafficRepository);
        var traffic = await trafficMeasurementService.GetTrafficInfo(trafficRepository, ct);
        return traffic;
    }
}