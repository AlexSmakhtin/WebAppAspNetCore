using Domain.Exceptions;
using Domain.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.ComponentModel.DataAnnotations;

namespace Backend.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }
        [HttpPost("registr")]
        public async Task<ActionResult<Account>> Register(
            RegistrationRequest registrationRequest,
            CancellationToken token)
        {
            try
            {
                _logger.LogInformation("{@registrationRequest}", registrationRequest);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var newAccount = await _accountService.Register(registrationRequest, token);
                return newAccount;
            }
            catch (EmailAlreadyExistsException)
            {
                return BadRequest("EmailAlreadyExistsException");
            }
            catch (InvalidOperationException)
            {
                return BadRequest("InvalidOperationException");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
