using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.Decorators;
using Presentation.Security;

namespace Presentation.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/products")]
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
        [Route("")]
        [LimitRequest(MaxRequests = 2, TimeWindow = 5)]
        public ActionResult<string> Products()
        {
            return "User, Admin, Manager e SysAdmin";
        }
    }
}
