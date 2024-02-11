using System.Security.Claims;
using ApiGateway.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using MySuperShop.Domain.Exceptions;
using MySuperShop.Domain.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using MySuperShop.Domain.Entities;
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
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("{request}", request);
                var newAccount = await _accountService.Register(
                    request.Name,
                    request.Email,
                    request.Password,
                    ct);
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
            [FromServices] JwtService jwtService,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("{request}", request);
                var existedAccount = await _accountService.Authenticate(
                    request.Email,
                    request.Password,
                    ct);
                var jwt = jwtService.GenerateToken(existedAccount);
                return new AuthenticationResponse(existedAccount.EmailAddress, jwt);
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

        [Authorize]
        [HttpGet("get_current")]
        public async Task<ActionResult<Account>> GetCurrentAccount(CancellationToken ct)
        {
            var userIdStr = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
                return NotFound("User id not found");
            var accId = Guid.Parse(userIdStr);
            var currentAccount = await _accountService.GetCurrentAccount(accId, ct);
            return currentAccount;
        }
    }
}
