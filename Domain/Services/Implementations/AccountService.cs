using Domain.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Services.Interfaces;
using Models;

namespace Domain.Services.Implementations
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AccountService(IAccountRepository repository, IPasswordHasher passwordHasher)
        {
            _accountRepository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Account> Register(
            RegistrationRequest registrationRequest,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(registrationRequest);
            var existedAccount = 
                await _accountRepository.FindByEmail(registrationRequest.Email, token);
            if (existedAccount != null)
            {
                throw new EmailAlreadyExistsException(nameof(existedAccount.EmailAddress));
            }
            var newAccount = new Account()
            {
                EmailAddress = registrationRequest.Email,
                Password = _passwordHasher.GenerateHash(registrationRequest.Password),
                Name = registrationRequest.Name,
            };
            await _accountRepository.Add(newAccount, token);
            return newAccount;
        }
    }
}
