using Application.Features.AuthenticationFeatures.DataTransferObjects;
using Application.Features.AuthenticationFeatures.Responses;
using Application.Interfaces.Contracts;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Documancer.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : BaseAPIController
    {
        #region Fields

        private readonly IUserRepository _user;

        #endregion

        #region Constructors
        
        public UserController(IUserRepository user)
        {
            _user = user;
        }

        #endregion

        #region Methods

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LogUserIn(LoginUserDTO loginUserDTO)
        {
            return Ok(await _user.LoginUserAsync(loginUserDTO));
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> RegisterUser(RegisterUserDTO registerUserDTO)
        {
            return Ok(await _user.RegisterUserAsync(registerUserDTO));
        }

        [HttpPost("update")]
        public async Task<ActionResult<UpdateUserCampaignsResponse>> UpdateUserCampaigns(UpdateUserCampaignsDTO updateUserDTO)
        {
            return Ok(await _user.UpdateUserCampaignsAsync(updateUserDTO));
        }

        #endregion
    }
}
