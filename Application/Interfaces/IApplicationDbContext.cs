using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Campaign> Campaigns { get; set; }

        Task<int> SaveChangesAsync();
    }
}
