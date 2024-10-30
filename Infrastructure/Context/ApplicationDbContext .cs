using Application.Interfaces;
using Domain.Entities.Authentication;
using Domain.Entities.Campaigns;
using Domain.Entities.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace Infrastructure.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        #region Properties and Fields

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Session> Sessions { get; set; }
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

            #region User

            // --- ApplicationUser Campaign
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Campaigns)
                .WithOne(e => e.OwnerUser)
                .HasForeignKey(e => e.OwnerUserId)
                .IsRequired(true);

            #endregion

            #region Campaign

            // --- Campaign ApplicationUser
            modelBuilder.Entity<Campaign>()
                .HasOne(e => e.OwnerUser)
                .WithMany(e => e.Campaigns)
                .HasForeignKey(e => e.OwnerUserId)
                .IsRequired(true);

            // --- Campaign Image
            modelBuilder.Entity<Campaign>()
                .HasOne(e => e.BannerImage)
                .WithOne(e => e.OwnerCampaign)
                .HasForeignKey<Image>(e => e.OwnerCampaignId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Campaign Session
            modelBuilder.Entity<Campaign>()
                .HasMany(e => e.Sessions)
                .WithOne(e => e.OwnerCampaign)
                .HasForeignKey(e => e.OwnerCampaignId)
                .IsRequired(false);

            #endregion

            #region Image

            // --- Image Campaign
            modelBuilder.Entity<Image>()
                .HasOne(e => e.OwnerCampaign)
                .WithOne(e => e.BannerImage)
                .HasForeignKey<Image>(e => e.OwnerCampaignId)
                .IsRequired(false);

            #endregion

            #region Sessions

            // --- Sessions Campaigns
            modelBuilder.Entity<Session>()
                .HasOne(e => e.OwnerCampaign)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.OwnerCampaignId)
                .IsRequired(true);

            #endregion
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        #endregion
    }
}
