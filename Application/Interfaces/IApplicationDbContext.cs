using Domain.Entities.Authentication;
using Domain.Entities.Campaign;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {

        #region Properties

        DbSet<Campaign> Campaigns { get; set; }
        DbSet<ApplicationUser> Users { get; set; }

        #endregion

        #region Methods

        Task<int> SaveChangesAsync();

        #endregion
    }
}
