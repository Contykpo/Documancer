using Documancer.Application.Features.Tenants.DTOs;

namespace Documancer.Application.Common.Interfaces.MultiTenant
{
    public interface ITenantService
    {
        #region Events

        event Func<Task>? OnChange;

        #endregion

        #region Properties

        List<TenantDto> DataSource { get; }

        #endregion

        #region Methods

        void Initialize();
        void Refresh();

        #endregion
    }
}