using Domain.Repositories.Interfaces;
using Models;

namespace Domain.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository repository)
        {
            _accountRepository = repository;
        }

        public async Task<Account> Register(
            string name,
            string email,
            string password,
            CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException($"'{nameof(email)}' cannot be null or whitespace.", nameof(email));
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException($"'{nameof(password)}' cannot be null or whitespace.", nameof(password));
            }
            var existedAccount = await _accountRepository.FindByEmail(email, token);
            if (existedAccount != null)
            {
                throw new InvalidOperationException("Email is already used");
            }
            var newAccount = new Account()
            {
                Id = Guid.NewGuid(),
                EmailAddress = email,
                Password = password,
                Name = name,
            };
            await _accountRepository.Add(newAccount, token);
            return newAccount;
        }
    }
}
