namespace Documancer.Application.DataTransferObjects.Response.Account
{
    public class GetUsersWithRolesResponseDTO
    {
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public string? RoleName { get; set; }
        public string? RoleId { get; set; }
    }
}
