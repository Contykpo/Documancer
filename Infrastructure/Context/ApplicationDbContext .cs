using Application.Interfaces;
using Domain.Entities.Authentication;
using Domain.Entities.Campaign;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        #region Properties and Fields

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        #endregion

        #region Constructors

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #endregion

        #region Methods
        
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        #endregion
    }
}
