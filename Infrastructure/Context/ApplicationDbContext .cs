using Application.Interfaces;
using Domain.Entities.Authentication;
using Domain.Entities.Campaigns;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Campaigns)
                .WithOne(e => e.OwnerUser)
                .HasForeignKey(e => e.OwnerUser.Id)
                .HasPrincipalKey(e => e.Id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        #endregion
    }
}
