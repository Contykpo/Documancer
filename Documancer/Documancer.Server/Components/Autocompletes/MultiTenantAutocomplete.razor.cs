using Documancer.Application.Common.Interfaces.MultiTenant;
using Documancer.Application.Features.Tenants.DTOs;

namespace Documancer.Server.Components.Autocompletes
{
    public class MultiTenantAutocomplete<T> : MudAutocomplete<TenantDto>
    {
        #region Properties and Fields

        [Inject]
        private ITenantService TenantsService { get; set; } = default!;

        #endregion

        #region Constructors

        public MultiTenantAutocomplete()
        {
            SearchFunc = SearchKeyValues;
            ToStringFunc = dto => dto?.Name;
            Clearable = true;
            Dense = true;
            ResetValueOnEmptyText = true;
            ShowProgressIndicator = true;
        }

        #endregion

        #region Methods

        protected override void OnInitialized()
        {
            base.OnInitialized();
            TenantsService.OnChange += TenantsService_OnChange;
        }

        private async Task TenantsService_OnChange()
        {
            await InvokeAsync(StateHasChanged);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TenantsService.OnChange -= TenantsService_OnChange;
            }
            base.Dispose(disposing);
        }

        private Task<IEnumerable<TenantDto>> SearchKeyValues(string value, CancellationToken cancellation)
        {
            IEnumerable<TenantDto> result;

            if (string.IsNullOrWhiteSpace(value))
            {
                result = TenantsService.DataSource.ToList();
            }
            else
            {
                result = TenantsService.DataSource
                    .Where(x => x.Name?.Contains(value, StringComparison.InvariantCultureIgnoreCase) == true ||
                                x.Description?.Contains(value, StringComparison.InvariantCultureIgnoreCase) == true)
                    .ToList();
            }

            return Task.FromResult(result);
        }

        #endregion
    }

}