using MySuperShop.Domain.Repositories.Interfaces;
using MySuperShop.Domain.Services;

namespace MySuperShop.ApiGateway.Middleware;

public class TrafficCounterMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TrafficCounterMiddleware> _logger;
    private readonly ITrafficMeasurementService _trafficMeasurementService;

    public TrafficCounterMiddleware(
        RequestDelegate next,
        ILogger<TrafficCounterMiddleware> logger,
        ITrafficMeasurementService trafficMeasurementService)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _trafficMeasurementService = trafficMeasurementService ??
                                     throw new ArgumentNullException(nameof(trafficMeasurementService));
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITrafficRepository trafficRepository)
    {
        ArgumentNullException.ThrowIfNull(trafficRepository);
        await _trafficMeasurementService.AddOrUpdate(context.Request.Path, trafficRepository, CancellationToken.None);
        await _next(context);
    }
}