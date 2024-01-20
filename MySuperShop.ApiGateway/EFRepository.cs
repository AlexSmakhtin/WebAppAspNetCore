using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MySuperShop.ApiGateway
{
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly MyDbContext _dbContext;

        private DbSet<TEntity> _entities => _dbContext.Set<TEntity>();

        public EFRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(TEntity entity, CancellationToken token)
        {
            await _entities.AddAsync(entity, token);
            await _dbContext.SaveChangesAsync(token);
        }

        public async Task<IReadOnlyCollection<TEntity>> GetAll(CancellationToken token)
        {
            return await _entities.ToListAsync(token);
        }

        public Task<TEntity> GetById(Guid Id, CancellationToken token)
        {
            return _entities.FirstAsync(e => e.Id == Id, token);
        }

        public async Task Update(TEntity entity, CancellationToken token)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(token);
        }

        public async Task Delete(TEntity entity, CancellationToken token)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync(token);
        }
    }
}
