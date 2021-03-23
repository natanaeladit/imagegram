using Imagegram.Application.Accounts.Commands.CreateAccount;
using Imagegram.Application.Accounts.Commands.Login;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Imagegram.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ApiControllerBase
    {
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] CreateAccountCommand createAccount)
        {
            var result = await Mediator.Send(createAccount);
            return result != null ? Created("", result) : (IActionResult)BadRequest(result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand login)
        {
            var result = await Mediator.Send(login);
            return result != null ? Created("", result) : (IActionResult)BadRequest(result);
        }
    }
}
