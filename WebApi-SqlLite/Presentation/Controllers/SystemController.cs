using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Presentation.Controllers
{
    /// <summary>
    /// A Controller class with several default endpoint related to system error handling.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SystemController : ControllerBase
    {
        /// <summary>
        /// An endpoint to handle with errors captured on development environment
        /// </summary>
        /// <param name="hostEnvironment"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/error-development")]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                return NotFound();
            }

            var exceptionHandlerFeature =
                HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            return Problem(
                detail: exceptionHandlerFeature.Error.StackTrace,
                title: exceptionHandlerFeature.Error.Message);
        }

        private IActionResult Problem(string? detail, string title)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/error")]
        public IActionResult HandleError()
        {
            return NotFound();//Problem();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/healthz")]
        public IActionResult HealthCheck()
        
        {

            return NotFound();
        }
    }
}
