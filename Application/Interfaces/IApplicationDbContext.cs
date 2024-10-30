using Domain.Entities.Authentication;
using Domain.Entities.Campaigns;
using Domain.Entities.Files;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {

        #region Properties

        DbSet<Campaign> Campaigns { get; set; }
        DbSet<Session> Sessions { get; set; }
        DbSet<ApplicationUser> Users { get; set; }
        DbSet<Image> Images { get; set; }

        #endregion

        #region Methods

        Task<int> SaveChangesAsync();

        #endregion
    }
}
