using ApiGateway.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySuperShop.ApiGateway.DbContexts;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories.Interfaces;
using MySuperShop.Domain.Services;

namespace ApiGateway.Services.Implementations;

public class TrafficMeasurementService : ITrafficMeasurementService
{
    private readonly ILogger<TrafficMeasurementService> _logger;

    public TrafficMeasurementService(ILogger<TrafficMeasurementService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddOrUpdate(string path, DbContext dbContext, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(path);
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path must be not empty", path);
        await using var transaction = await dbContext.Database.BeginTransactionAsync(ct);
        try
        {
            var dbSet = dbContext.Set<TrafficInfo>();
            var existedTrafficIfo = await dbSet.FirstOrDefaultAsync(e => e.Path == path, ct);
            if (existedTrafficIfo == null)
            {
                var trafficInfo = new TrafficInfo(path);
                await dbSet.AddAsync(trafficInfo, ct);
                await dbContext.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return;
            }

            existedTrafficIfo.CountOfVisits++;
            dbContext.Entry(existedTrafficIfo).State = EntityState.Modified;
            await dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Transaction error: {ex.Message}", ex.Message);
            await transaction.RollbackAsync(ct);
        }
    }

    public async Task<IReadOnlyCollection<TrafficInfo>> GetTrafficInfo(ITrafficRepository trafficRepository, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(trafficRepository);
        var collection = await trafficRepository.GetAll(ct);
        return collection;
    }
}