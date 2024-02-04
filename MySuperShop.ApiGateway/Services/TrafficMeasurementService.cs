using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories.Interfaces;
using MySuperShop.Domain.Services;

namespace MySuperShop.ApiGateway.Services;

public class TrafficMeasurementService : ITrafficMeasurementService
{
    private readonly ILogger<TrafficMeasurementService> _logger;
    private readonly SemaphoreSlim _semaphoreSlim = new(1);

    public TrafficMeasurementService(ILogger<TrafficMeasurementService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddOrUpdate(string path, ITrafficRepository trafficRepository, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(path);
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path must be not empty", path);
        await _semaphoreSlim.WaitAsync(ct);
        try
        {
            var existedTrafficIfo = await trafficRepository.FindByPath(path, ct);
            if (existedTrafficIfo == null)
            {
                var trafficInfo = new TrafficInfo(path);
                await trafficRepository.Add(trafficInfo, ct);
                return;
            }
            existedTrafficIfo.CountOfVisits++;
            await trafficRepository.Update(existedTrafficIfo, ct);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task<List<TrafficInfo>> GetTrafficInfo(ITrafficRepository trafficRepository, CancellationToken ct)
    {
        await _semaphoreSlim.WaitAsync(ct);
        try
        {
            var collection = await trafficRepository.GetAll(ct);
            return collection.ToList();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
}