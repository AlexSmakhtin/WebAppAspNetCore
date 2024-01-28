using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories.Interfaces;

namespace MySuperShop.Domain.Services;

public interface ITrafficMeasurementService
{
    Task AddOrUpdate(string path, ITrafficRepository trafficRepository, CancellationToken ct);
    Task<List<TrafficInfo>> GetTrafficInfo(ITrafficRepository trafficRepository, CancellationToken ct);
}