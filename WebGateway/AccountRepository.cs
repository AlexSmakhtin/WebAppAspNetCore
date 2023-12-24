using Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebGateway
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyDbContext _dbContext;
        private DbSet<Account> _accounts => _dbContext.Set<Account>();

        public AccountRepository(MyDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task Add(Account entity, CancellationToken token)
        {
            await _accounts.AddAsync(entity, token);
            await _dbContext.SaveChangesAsync(token);
        }

        public async Task<Account?> FindByEmail(string email, CancellationToken token)
        {
            var account = await _accounts
                .FirstOrDefaultAsync(e => e.EmailAddress == email, token);
            return account;
        }

        public async Task<IReadOnlyCollection<Account>> GetAll(CancellationToken token)
        {
            return await _accounts.ToListAsync(token);
        }

        public Task<Account> GetById(Guid Id, CancellationToken token)
        {
            return _accounts.FirstAsync(e => e.Id == Id, token);
        }

        public async Task Update(Account entity, CancellationToken token)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(token);
        }

        public async Task Delete(Account entity, CancellationToken token)
        {
            _accounts.Remove(entity);
            await _dbContext.SaveChangesAsync(token);
        }
    }
}
