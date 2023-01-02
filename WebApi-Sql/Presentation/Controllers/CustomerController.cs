using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModel;
//using System.Web.Http;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]  
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            return await Task.FromResult<string>("value1");
        }

        // GET: api/values/5
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Get(int id)
        {
            return await Task.FromResult<string>("value1");
        }

        // POST: api/values
        [HttpPost]
        [Route("{customer}")]
        public async Task<ActionResult<Guid>> Post([FromBody] CustomerRequest customer)
        {

            return await Task.FromResult<Guid>(Guid.NewGuid());
        }

        // PUT: api/values/5
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Guid>> Put(int id, [FromBody] string value)
        {
            return await Task.FromResult<Guid>(Guid.NewGuid());
        }

        // DELETE: api/values/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Guid>> Delete(int id)
        {
            return await Task.FromResult<Guid>(Guid.NewGuid());
        }
    }
}
