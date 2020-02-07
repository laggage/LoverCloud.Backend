namespace LoverCloud.Infrastructure.Database.Configurations
{
    using LoverCloud.Core.Extensions;
    using LoverCloud.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class LoverConfiguration : IEntityTypeConfiguration<Lover>
    {
        public void Configure(EntityTypeBuilder<Lover> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasMaxLength(LoverCloudApiConstraint.IdLength);
            builder.ToTable(nameof(Lover));
        }
    }

    internal class LoverLogConfiguration : IEntityTypeConfiguration<LoverLog>
    {
        public void Configure(EntityTypeBuilder<LoverLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasMaxLength(LoverCloudApiConstraint.IdLength);
            builder.ToTable(nameof(LoverLog));
            builder.HasOne(o => o.Lover)
                .WithMany(o => o.LoverLogs)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.Content).HasMaxLength(1024);
        }
    }

    internal class LoverAnniversaryConfiguration : IEntityTypeConfiguration<LoverAnniversary>
    {
        public void Configure(EntityTypeBuilder<LoverAnniversary> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasMaxLength(LoverCloudApiConstraint.IdLength);
            builder.ToTable(nameof(LoverAnniversary));
            builder.Property(x => x.Name)
                .HasMaxLength(LoverAnniversary.NameMaxLength);
            builder.Property(x => x.Description)
                .HasMaxLength(LoverAnniversary.DescriptionMaxLength);
            builder.Property(x => x.Date).HasColumnType("date");
            builder.HasOne(o => o.Lover)
                .WithMany(o => o.LoverAnniversaries)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
