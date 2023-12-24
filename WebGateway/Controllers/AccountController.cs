using Domain.Entities;
using Domain.Exceptions;
using Domain.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using HttpModels;

namespace WebGateway.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            AccountService accountService,
            ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<ActionResult<Account>> Register(
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
                return newAccount;
            }
            catch (EmailAlreadyExistsException)
            {
                return BadRequest("EmailAlreadyExistsException");
            }
        }
    }
}
