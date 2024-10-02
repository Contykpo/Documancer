using Application.Features.CampaignFeatures.Commands.Create;
using Application.Features.CampaignFeatures.Commands.Delete;
using Application.Features.CampaignFeatures.Commands.Update;
using Application.Features.CampaignFeatures.Queries.Get;
using Application.Features.CampaignFeatures.Queries.List;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Documancer.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CampaignController : BaseAPIController
    {
        /// <summary>
        /// Creates a New Product.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateCampaignCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// Gets all Products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new ListCampaignsQuery()));
        }
        /// <summary>
        /// Gets Product Entity by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await Mediator.Send(new GetCampaignByIdQuery(id)));
        }
        /// <summary>
        /// Deletes Product Entity based on Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteCampaignByIdCommand(id)));
        }
        /// <summary>
        /// Updates the Product Entity based on Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(Guid id, UpdateCampaignCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }
    }
}
