using Application.Features.SessionFeatures.Commands.Create;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Documancer.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SessionController : BaseAPIController
    {
        /// <summary>
        /// Creates a New Session for a given campaign.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateSessionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}