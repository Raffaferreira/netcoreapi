using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using Presentation.ViewModel;
//using System.Web.Http;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Presentation.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomerController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CustomerResponse>> Get()
        {
            return await Mediator.Send(new CustomerRequest());

        }

        // GET: api/values/5
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Get(int id)
        {
            return await Task.FromResult("value1");
        }

        // POST: api/values
        [HttpPost]
        [Route("{customer}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<Guid>> Post([FromBody] CustomerRequest customer)
        {

            return await Task.FromResult(Guid.NewGuid());
        }

        // PUT: api/values/5
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<Guid>> Put(int id, [FromBody] string value)
        {
            return await Task.FromResult(Guid.NewGuid());
        }

        // DELETE: api/values/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Guid>> Delete(int id)
        {
            return await Task.FromResult(Guid.NewGuid());
        }
    }
}
