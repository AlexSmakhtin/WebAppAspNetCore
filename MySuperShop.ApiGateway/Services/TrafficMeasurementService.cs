using System.Collections.Concurrent;

namespace MySuperShop.ApiGateway.Services;

public class TrafficMeasurementService
{
    private readonly ConcurrentDictionary<string, int> _pathCounter = new();
    private readonly ILogger<TrafficMeasurementService> _logger;
    private readonly object _lockObject = new();

    public TrafficMeasurementService(ILogger<TrafficMeasurementService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void TryAddOrUpdate(PathString pathString)
    {
        ArgumentNullException.ThrowIfNull(pathString);
        _pathCounter.AddOrUpdate(pathString.ToString(), 1, (path, count) => count + 1);
    }

    public Dictionary<string, int> GetTraffic()
    {
        lock (_lockObject)
        {
            return new Dictionary<string, int>(_pathCounter);
        }
    }
}