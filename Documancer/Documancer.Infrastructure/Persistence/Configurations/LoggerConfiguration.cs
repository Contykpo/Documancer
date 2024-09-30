using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Documancer.Infrastructure.Persistence.Configurations
{
    public class LoggerConfiguration : IEntityTypeConfiguration<Logger>
    {
        public void Configure(EntityTypeBuilder<Logger> builder)
        {
            builder.Property(x => x.Level).HasMaxLength(450);
            builder.Property(x => x.Message).HasMaxLength(int.MaxValue);
            builder.Property(x => x.Exception).HasMaxLength(int.MaxValue);
            builder.Property(x => x.MessageTemplate).HasMaxLength(int.MaxValue);
            builder.Property(x => x.Properties).HasMaxLength(int.MaxValue);
            builder.Property(x => x.LogEvent).HasMaxLength(int.MaxValue);
            builder.HasIndex(x => new { x.Level });
            builder.HasIndex(x => x.TimeStamp);

        }
    }
}