using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.Controllers.Base;
using Presentation.ViewModel;

namespace Presentation.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/blacklist")]
    public class BlackListController : BaseController
    {
        private readonly ApplicationSetup _options;
        private readonly TopItemSettings _yearTopItem;
        private readonly TopItemSettings _monthTopItem;

        public BlackListController(IOptions<ApplicationSetup> options,
            IOptionsSnapshot<TopItemSettings> namedOptionsAccessor)
        {
            _options = options.Value;
            _monthTopItem = namedOptionsAccessor.Get(TopItemSettings.Month);
            _yearTopItem = namedOptionsAccessor.Get(TopItemSettings.Year);
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<CustomerResponse>> Get()
        {
            return await Mediator.Send(new CustomerRequest());
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
        [Authorize(Policy = "EnableToAll")]
        public string AllRoles() => "User, Admin, Manager e SysAdmin";

        [HttpGet]
        [Route("user")]
        [Authorize(Policy = "UserOnly")]
        public string Users() => "Usuário";

        [HttpGet]
        [Route("admin")]
        [Authorize(Policy = "AdminOnly")]
        public string Adim() => "Administrator";

        [HttpGet]
        [Route("manager")]
        [Authorize(Policy = "ManagerOnly")]
        public string Manager() => "Manager";

        [HttpGet]
        [Route("sysadmin")]
        [Authorize(Policy = "SysAdminOnly")]
        public string SysAdmin() => "SysAdministrator";
    }
}
