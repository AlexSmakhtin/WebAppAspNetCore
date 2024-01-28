using Microsoft.EntityFrameworkCore;
using MySuperShop.ApiGateway.DbContexts;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories.Interfaces;

namespace MySuperShop.ApiGateway.Repositories
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly MyDbContext _dbContext;

        private DbSet<TEntity> Entities => _dbContext.Set<TEntity>();

        public EfRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(TEntity entity, CancellationToken ct)
        {
            await Entities.AddAsync(entity, ct);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyCollection<TEntity>> GetAll(CancellationToken ct)
        {
            return await Entities.ToListAsync(ct);
        }

        public Task<TEntity> GetById(Guid id, CancellationToken ct)
        {
            return Entities.FirstAsync(e => e.Id == id, ct);
        }

        public async Task Update(TEntity entity, CancellationToken ct)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task Delete(TEntity entity, CancellationToken ct)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}