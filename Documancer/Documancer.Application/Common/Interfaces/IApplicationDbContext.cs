using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Documancer.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        #region Properties

        DbSet<Logger> Loggers { get; set; }
        DbSet<AuditTrail> AuditTrails { get; set; }
        DbSet<Document> Documents { get; set; }
        DbSet<KeyValue> KeyValues { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Tenant> Tenants { get; set; }
        DbSet<Contact> Contacts { get; set; }
        ChangeTracker ChangeTracker { get; }

        DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        #endregion

        #region Methods

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        #endregion
    }
}