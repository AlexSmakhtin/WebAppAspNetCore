using Microsoft.EntityFrameworkCore;
using MySuperShop.ApiGateway.DbContexts;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories.Interfaces;

namespace MySuperShop.ApiGateway.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyDbContext _dbContext;
        private DbSet<Account> Accounts => _dbContext.Set<Account>();

        public AccountRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Account entity, CancellationToken ct)
        {
            await Accounts.AddAsync(entity, ct);
            await _dbContext.SaveChangesAsync(ct);
        }

        public Task<Account?> FindByEmail(string email, CancellationToken ct)
        {
            return Accounts
                .FirstOrDefaultAsync(e => e.EmailAddress == email, ct);
        }

        public async Task<IReadOnlyCollection<Account>> GetAll(CancellationToken ct)
        {
            return await Accounts.ToListAsync(ct);
        }

        public Task<Account> GetById(Guid id, CancellationToken ct)
        {
            return Accounts.FirstAsync(e => e.Id == id, ct);
        }

        public async Task Update(Account entity, CancellationToken ct)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task Delete(Account entity, CancellationToken ct)
        {
            Accounts.Remove(entity);
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}