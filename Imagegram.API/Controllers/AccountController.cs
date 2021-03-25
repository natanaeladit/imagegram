using Imagegram.Application.Accounts.Commands.CreateAccount;
using Imagegram.Application.Accounts.Commands.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Imagegram.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ApiControllerBase
    {
        /// <summary>
        /// Create new account
        /// </summary>
        /// <param name="createAccount"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateAccountVm>> Register([FromBody] CreateAccountCommand createAccount)
        {
            var result = await Mediator.Send(createAccount);
            return result?.Result == true ? Created("", result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Get bearer token from user credentials
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginVm>> Login([FromBody] LoginCommand login)
        {
            var result = await Mediator.Send(login);
            return result.Result == true ? Created("", result) : (ActionResult)BadRequest(result);
        }
    }
}
