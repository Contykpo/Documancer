using Documancer.Application.Common.Interfaces.Identity;
using Documancer.Application.Features.Identity.DTOs;

namespace Documancer.Server.Components.Autocompletes
{
    public class PickUserAutocomplete<T> : MudAutocomplete<ApplicationUserDto>
    {
        #region Properties and Fields

        [Parameter]
        public string? TenantId { get; set; }

        [Inject]
        private IUserService UserService { get; set; } = default!;

        #endregion

        #region Constructors

        public PickUserAutocomplete()
        {
            SearchFunc = SearchKeyValues;
            ToStringFunc = dto => dto?.UserName;
            Clearable = true;
            Dense = true;
            ResetValueOnEmptyText = true;
            ShowProgressIndicator = true;
            MaxItems = 50;
        }

        #endregion

        #region Methods

        protected override void OnInitialized()
        {
            UserService.OnChange += TenantsService_OnChange;
        }

        private async Task TenantsService_OnChange()
        {
            await InvokeAsync(StateHasChanged);
        }

        protected override void Dispose(bool disposing)
        {
            UserService.OnChange -= TenantsService_OnChange;
            base.Dispose(disposing);
        }

        private Task<IEnumerable<ApplicationUserDto>> SearchKeyValues(string value, CancellationToken cancellation)
        {
            IEnumerable<ApplicationUserDto> result = UserService.DataSource.Where(x => x.TenantId.Equals(TenantId));

            if (!string.IsNullOrEmpty(value))
            {
                result = UserService.DataSource.Where(x => x.TenantId.Equals(TenantId) &&
                            (x.UserName.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                            x.Email.Contains(value, StringComparison.OrdinalIgnoreCase)));
            }
            return Task.FromResult(result);
        }

        #endregion
    }
}