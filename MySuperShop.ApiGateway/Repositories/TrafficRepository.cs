using Microsoft.EntityFrameworkCore;
using MySuperShop.ApiGateway.DbContexts;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories.Interfaces;

namespace MySuperShop.ApiGateway.Repositories;

public class TrafficRepository : ITrafficRepository
{
    private readonly MyDbContext _dbContext;

    private DbSet<TrafficInfo> TrafficInfos => _dbContext.Set<TrafficInfo>();

    public TrafficRepository(MyDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Task<TrafficInfo> GetById(Guid id, CancellationToken ct)
    {
        return TrafficInfos.FirstAsync(e => e.Id == id, ct);
    }

    public async Task<IReadOnlyCollection<TrafficInfo>> GetAll(CancellationToken ct)
    {
        return await TrafficInfos.ToListAsync(ct);
    }

    public async Task Add(TrafficInfo entity, CancellationToken ct)
    {
        await TrafficInfos.AddAsync(entity, ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task Update(TrafficInfo entity, CancellationToken ct)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task Delete(TrafficInfo entity, CancellationToken ct)
    {
        TrafficInfos.Remove(entity);
        await _dbContext.SaveChangesAsync(ct);
    }

    public Task<TrafficInfo?> FindByPath(string path, CancellationToken ct)
    {
        return TrafficInfos.FirstOrDefaultAsync(e => e.Path == path, ct);
    }
}