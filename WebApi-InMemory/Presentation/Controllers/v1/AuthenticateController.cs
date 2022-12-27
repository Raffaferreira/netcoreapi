using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.Dependencies;
using Presentation.Security;

namespace Presentation.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/authenticate")]
    public class AuthenticateController : ControllerBase
    {
        private readonly ApplicationOptionsConfiguration _options;
        private readonly TopItemSettings _yearTopItem;
        private readonly TopItemSettings _monthTopItem;

        public AuthenticateController(IOptions<ApplicationOptionsConfiguration> options,
            IOptionsSnapshot<TopItemSettings> namedOptionsAccessor)
        {
            _options = options.Value;
            _monthTopItem = namedOptionsAccessor.Get(TopItemSettings.Month);
            _yearTopItem = namedOptionsAccessor.Get(TopItemSettings.Year);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<UserResponse> Authenticate([FromBody] UserRequest model)
        {
            var service = new AuthenticationService();
            var user = service.Authenticate(model);

            return Ok(user);
        }

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public async Task<ActionResult<string>> Authenticated()
        {
            return await Task.FromResult(string.Format("User '{0}' Authenticated", User.Identity!.Name));
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("all")]
        [Authorize(Roles = "user,admin")]
        public string Employee() => "Usuário e Administrator";

        [HttpGet]
        [Route("user")]
        [Authorize(Roles = "user")]
        public string Users() => "Usuário";

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "admin")]
        public string Manager() => "Administrator";
    }
}
