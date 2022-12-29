using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Base
{
    [Controller]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BaseController : ControllerBase
    {
        private IMediator? _mediator;

        protected IMediator Mediator
        {
            get
            {
                return _mediator = HttpContext.RequestServices.GetService<IMediator>()!;
            }
        }

        private ISender? _mediatorSender;

        protected ISender MediatorSender
        {
            get
            {
                return _mediatorSender = HttpContext.RequestServices.GetService<ISender>()!;
            }
        }

    }
}
