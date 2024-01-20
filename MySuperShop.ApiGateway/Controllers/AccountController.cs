using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Exceptions;
using MySuperShop.Domain.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using MySuperShop.HttpModels;

namespace MySuperShop.ApiGateway.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            AccountService accountService,
            ILogger<AccountController> logger
           )
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> Register(
            RegistrationRequest request,
            CancellationToken token)
        {
            try
            {
                _logger.LogInformation("{request}", request);
                var newAccount = await _accountService.Register(
                    request.Name,
                    request.Email,
                    request.Password,
                    token);
                return new RegistrationResponse(newAccount.Name, newAccount.EmailAddress);
            }
            catch (EmailAlreadyExistsException)
            {
                return BadRequest("Email already used");
            }
        }
        
        [HttpPost("authentication")]
        public async Task<ActionResult<AuthenticationResponse>> Authentication(
            AuthenticationRequest request,
            CancellationToken token)
        {
            try
            {
                _logger.LogInformation("{request}", request);
                var existedAccount = await _accountService.Authenticate(
                    request.Email,
                    request.Password,
                    token);
                return new AuthenticationResponse(existedAccount.EmailAddress);
            }
            catch (AccountNotFoundException)
            {
                return Unauthorized("Account not found");
            }
            catch (IncorrectPasswordException)
            {
                return Unauthorized("Invalid password");
            }
        }
    }
}
