using Application.Features.CampaignFeatures.Commands.Create;
using Application.Features.CampaignFeatures.Commands.Delete;
using Application.Features.CampaignFeatures.Commands.Update;
using Application.Features.CampaignFeatures.Queries.Get;
using Application.Features.CampaignFeatures.Queries.List;
using Application.Features.FilesFeatures.Commands;
using Asp.Versioning;
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
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateCampaignCommand command, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("No file uploaded.");

            using var ms = new MemoryStream();

            await file.CopyToAsync(ms);

            var fileBytes = ms.ToArray();

            var command = new UploadImageCommand(file.FileName, file.ContentType, fileBytes);

            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Uploads a New Image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadImage()
        {


            return Ok(await Mediator.Send(command));
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
