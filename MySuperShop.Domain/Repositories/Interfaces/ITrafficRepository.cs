using MySuperShop.Domain.Entities;

namespace MySuperShop.Domain.Repositories.Interfaces;

public interface ITrafficRepository : IRepository<TrafficInfo>
{
    Task<TrafficInfo?> FindByPath(string path, CancellationToken ct);
}