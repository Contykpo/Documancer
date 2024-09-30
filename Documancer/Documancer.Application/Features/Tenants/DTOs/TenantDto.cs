namespace Documancer.Application.Features.Tenants.DTOs
{
    [Description("Tenants")]
    public class TenantDto
    {
        #region Properties and Fields

        [Description("Tenant Id")] public string Id { get; set; } = Guid.NewGuid().ToString();

        [Description("Tenant Name")] public string? Name { get; set; }

        [Description("Description")] public string? Description { get; set; }

        #endregion

        #region Constructors

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Tenant, TenantDto>().ReverseMap();
            }
        }

        #endregion
    }
}