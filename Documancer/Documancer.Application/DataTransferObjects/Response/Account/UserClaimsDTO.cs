namespace Documancer.Application.DataTransferObjects.Response.Account
{
    public record UserClaimsDTO(string Fullname = null!, string Username = null!, string EmailAddress = null!, string Role = null!);
}
