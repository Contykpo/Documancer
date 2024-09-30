using Documancer.Application.Common.Interfaces.Identity;
using Documancer.Application.Features.Identity.DTOs;

namespace Documancer.Server.Components.Autocompletes;

public class PickSuperiorIdAutocomplete<T> : MudAutocomplete<ApplicationUserDto>
{
    #region Properties and Fields

    [Parameter]
    public string? TenantId { get; set; }
    [Parameter]
    public string? OwnerName { get; set; }

    [Inject]
    private IUserService UserService { get; set; } = default!;

    #endregion

    #region Constructors

    public PickSuperiorIdAutocomplete()
    {
  
        SearchFunc = SearchKeyValues;
        ToStringFunc = dto=>dto?.UserName;
        Clearable = true;
        Dense = true;
        ResetValueOnEmptyText = true;
        ShowProgressIndicator = true;
        MaxItems = 200;
    }

    #endregion

    #region Methods

    private Task<IEnumerable<ApplicationUserDto>> SearchKeyValues(string value, CancellationToken cancellation)
    {
        IEnumerable<ApplicationUserDto> result= UserService.DataSource.Where(x => x.TenantId.Equals(TenantId) &&   !x.UserName.Equals(OwnerName));
        if (!string.IsNullOrWhiteSpace(value))
        {
            result = UserService.DataSource.Where(x => x.TenantId.Equals(TenantId) && !x.UserName.Equals(OwnerName)
                && (x.UserName.Contains(value, StringComparison.OrdinalIgnoreCase)
                || x.Email.Contains(value, StringComparison.OrdinalIgnoreCase)));
        }

        return Task.FromResult(result);
    }
    
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

    #endregion
}