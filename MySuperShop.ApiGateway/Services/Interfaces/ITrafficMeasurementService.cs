using Microsoft.EntityFrameworkCore;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories.Interfaces;

namespace ApiGateway.Services.Interfaces;

public interface ITrafficMeasurementService
{
    Task AddOrUpdate(string path, DbContext dbContext, CancellationToken ct);
    Task<IReadOnlyCollection<TrafficInfo>> GetTrafficInfo(ITrafficRepository trafficRepository, CancellationToken ct);
}