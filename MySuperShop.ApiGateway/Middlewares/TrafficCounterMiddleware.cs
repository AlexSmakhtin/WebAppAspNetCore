using MySuperShop.ApiGateway.Services;

namespace MySuperShop.ApiGateway.Middleware;

public class TrafficCounterMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TrafficCounterMiddleware> _logger;
    private readonly TrafficMeasurementService _trafficMeasurementService;

    public TrafficCounterMiddleware(
        RequestDelegate next,
        ILogger<TrafficCounterMiddleware> logger,
        TrafficMeasurementService trafficMeasurementService)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _trafficMeasurementService = trafficMeasurementService ??
                                     throw new ArgumentNullException(nameof(trafficMeasurementService));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _trafficMeasurementService.TryAddOrUpdate(context.Request.Path);
        await _next(context);
    }
}