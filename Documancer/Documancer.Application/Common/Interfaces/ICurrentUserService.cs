namespace Documancer.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? UserName { get; }
        string? TenantId { get; }
        string? TenantName { get; }
    }
}