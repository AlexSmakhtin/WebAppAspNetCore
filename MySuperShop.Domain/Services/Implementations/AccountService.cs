using MySuperShop.Domain.Exceptions;
using MySuperShop.Domain.Repositories.Interfaces;
using MySuperShop.Domain.Entities;

namespace MySuperShop.Domain.Services.Implementations
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasherService _passwordHasherService;

        public AccountService(IAccountRepository repository, IPasswordHasherService passwordHasherService)
        {
            _accountRepository = repository;
            _passwordHasherService = passwordHasherService;
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
                throw new EmailAlreadyExistsException("Email already used");
            var hashedPassword = _passwordHasherService.HashPassword(password);
            var newAccount = new Account(name, email, hashedPassword);
            await _accountRepository.Add(newAccount, token);
            return newAccount;
        }

        public async Task<Account> Authenticate(
            string email,
            string password,
            CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(password);
            var existedAccount =
                await _accountRepository.FindByEmail(email, token);
            if (existedAccount == null)
                throw new AccountNotFoundException("Account not found");
            var result = _passwordHasherService.VerifyPassword(existedAccount.HashedPassword, password);
            if (result == false)
                throw new IncorrectPasswordException("Password incorrect");
            return existedAccount;
        }
    }
}