using Application.Features.CampaignFeatures.Commands.Create;
using Application.Features.CampaignFeatures.Commands.Delete;
using Application.Features.CampaignFeatures.Commands.Update;
using Application.Features.CampaignFeatures.Queries.Get;
using Application.Features.CampaignFeatures.Queries.List;
using Application.Features.FilesFeatures.Commands;
using Asp.Versioning;
using Documancer.API.Features.Campaigns.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Documancer.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CampaignController : BaseAPIController
    {
        /// <summary>
        /// Creates a New Campaign with an optional image upload.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCampaignRequest request)
        {
            byte[]? imageData = null;
            string? fileName = null;
            string? contentType = null;

            if (request.BannerImageFile != null)
            {
                using var memoryStream = new MemoryStream();
                await request.BannerImageFile.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
                fileName = request.BannerImageFile.FileName;
                contentType = request.BannerImageFile.ContentType;
            }

            var command = new CreateCampaignCommand(
                request.Name,
                request.Description,
                fileName,
                contentType,
                imageData
            );

            var campaignId = await Mediator.Send(command);
            return Ok(campaignId);
        }


        /// <summary>
        /// Gets all Campaign.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new ListCampaignsQuery()));
        }
        /// <summary>
        /// Gets Campaign Entity by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await Mediator.Send(new GetCampaignByIdQuery(id)));
        }
        /// <summary>
        /// Deletes Campaign Entity based on Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteCampaignByIdCommand(id)));
        }
        /// <summary>
        /// Updates the CampaignEntity based on Id.
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
