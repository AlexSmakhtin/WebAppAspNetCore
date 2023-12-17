using Domain.Services;
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

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("registr")]
        public async Task<ActionResult<Account>> Register(
            [Required] string name,
            [EmailAddress] string email,
            [Required] string password,
            CancellationToken token)
        {
            try
            {
                var newAccount = await _accountService.Register(name, email, password, token);
                return newAccount;
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
        }
    }
}
