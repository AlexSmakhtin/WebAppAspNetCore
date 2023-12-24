using Domain.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Services.Interfaces;
using Domain.Entities;

namespace Domain.Services.Implementations
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
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(password);
            var existedAccount = 
                await _accountRepository.FindByEmail(email, token);
            if (existedAccount != null)
            {
                throw new EmailAlreadyExistsException("Email already used");
            }
            var newAccount = new Account(name, email, password);
            await _accountRepository.Add(newAccount, token);
            return newAccount;
        }
    }
}
