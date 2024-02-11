using ApiGateway.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySuperShop.ApiGateway.DbContexts;
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

    public async Task InvokeAsync(HttpContext httpContext, MyDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        await _trafficMeasurementService.AddOrUpdate(httpContext.Request.Path, dbContext, CancellationToken.None);
        await _next(httpContext);
    }
}