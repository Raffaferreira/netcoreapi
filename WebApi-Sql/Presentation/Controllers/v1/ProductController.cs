using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.Security;

namespace Presentation.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/product")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationSetup _options;
        private readonly TopItemSettings _yearTopItem;
        private readonly TopItemSettings _monthTopItem;

        public ProductController(IOptions<ApplicationSetup> options,
            IOptionsSnapshot<TopItemSettings> namedOptionsAccessor)
        {
            _options = options.Value;
            _monthTopItem = namedOptionsAccessor.Get(TopItemSettings.Month);
            _yearTopItem = namedOptionsAccessor.Get(TopItemSettings.Year);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Authenticated()
        {
            return await Task.FromResult(string.Format("User '{0}' Authenticated", User.Identity!.Name));
        }
    }
}
