using Application.Interfaces;
using Domain.Entities.Authentication;
using Domain.Entities.Campaigns;
using Domain.Entities.Files;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Infrastructure.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        #region Properties and Fields

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Image> Images { get; set; }

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

            // --- ApplicationUser Campaign
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Campaigns)
                .WithOne(e => e.OwnerUser)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Campaign ApplicationUser
            modelBuilder.Entity<Campaign>()
                .HasOne(e => e.OwnerUser)
                .WithMany(e => e.Campaigns)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Campaign Image
            modelBuilder.Entity<Campaign>()
                .HasOne(e => e.BannerImage)
                .WithOne(e => e.OwnerCampaign)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Image Campaign
            modelBuilder.Entity<Image>()
                .HasOne(e => e.OwnerCampaign)
                .WithOne(e => e.BannerImage)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        #endregion
    }
}
