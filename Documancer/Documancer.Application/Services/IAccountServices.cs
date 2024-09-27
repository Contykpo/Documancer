using Documancer.Application.DataTransferObjects.Request.Account;
using Documancer.Application.DataTransferObjects.Response.Account;
using Documancer.Application.DataTransferObjects.Response;

namespace Documancer.Application.Services
{
    public interface IAccountServices
    {
        Task CreateAdmin();       
        Task<GeneralResponse> CreateAccountAsync(CreateAccountDTO model);
        Task<LoginResponse> LoginAccountAsync(LoginDTO model);
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model);
        Task<GeneralResponse> CreateRoleAsync(CreateRoleDTO model);
        Task<IEnumerable<GetRoleDTO>> GetRolesAsync();
        Task<IEnumerable<GetUsersWithRolesResponseDTO>> GetUsersWithRolesAsync();
        Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model);
    }
}
