using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Documancer.Infrastructure.Persistence.Configurations
{
    #nullable disable
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
            builder.Ignore(e => e.DomainEvents);
        }
    }
}