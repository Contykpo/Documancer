using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Documancer.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseAPIController : ControllerBase
    {
        #region Properties and Fields

        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        #endregion
    }
}
