using Documancer.Application.Contracts;
using Documancer.Application.DataTransferObjects.Request.Account;
using Documancer.Application.DataTransferObjects.Response;
using Documancer.Application.DataTransferObjects.Response.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Documancer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccount account) : ControllerBase
    {
        [HttpPost("identity/create")]
        public async Task<ActionResult<GeneralResponse>> CreateAccount(CreateAccountDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model cannot be null.");
            }

            return Ok(await account.CreateAccountAsync(model));
        }

        [HttpPost("identity/login")]
        public async Task<ActionResult<GeneralResponse>> LoginAccount(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model cannot be null.");
            }

            return Ok(await account.LoginAccountAsync(model));
        }

        [HttpPost("identity/refresh-token")]
        public async Task<ActionResult<GeneralResponse>> RefreshToken(RefreshTokenDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model cannot be null.");
            }
            
            return Ok(await account.RefreshTokenAsync(model));
        }

        [HttpPost("identity/role/create")]
        public async Task<ActionResult<GeneralResponse>> CreateRole(CreateRoleDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model cannot be null.");
            }

            return Ok(await account.CreateRoleAsync(model));
        }

        [HttpPost("identity/role/list")]
        public async Task<ActionResult<IEnumerable<GetRoleDTO>>> GetRoles() => Ok(await account.GetRolesAsync());

        [HttpPost("/setting")]
        public async Task<IActionResult> CreateAdmin()
        {
            await account.CreateAdmin();

            return Ok();
        }

        [HttpPost("identity/user-with-roles")]
        public async Task<ActionResult<IEnumerable<GetUsersWithRolesResponseDTO>>> GetUserWithRoles() => Ok(await account.GetUsersWithRolesAsync());

        [HttpPost("identity/change-role")]
        public async Task<ActionResult<IEnumerable<GeneralResponse>>> ChangeUserRole(ChangeUserRoleRequestDTO model) => Ok(await account.ChangeUserRoleAsync(model));
    }
}
