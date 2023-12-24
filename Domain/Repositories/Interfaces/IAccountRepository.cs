using Domain.Entities;

namespace Domain.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account?> FindByEmail(string email, CancellationToken token);
    }
}
