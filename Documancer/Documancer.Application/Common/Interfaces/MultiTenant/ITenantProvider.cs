namespace Documancer.Application.Common.Interfaces.MultiTenant
{
    public interface ITenantProvider
    {
        #region Properties

        string? TenantId { get; set; }
        string? TenantName { get; set; }

        #endregion

        #region Methods

        void Update();
        Guid Register(Action callback);
        void Clear();
        void Unregister(Guid id);

        #endregion
    }
}