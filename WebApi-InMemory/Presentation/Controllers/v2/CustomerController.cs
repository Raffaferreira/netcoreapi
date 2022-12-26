using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;

namespace Presentation.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0", Deprecated = true)]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomerController : BaseController
    {
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Get(int id)
        {
            return await Task.FromResult("value1");
        }
    }
}
