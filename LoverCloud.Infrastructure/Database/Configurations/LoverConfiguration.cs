namespace LoverCloud.Infrastructure.Database.Configurations
{
    using LoverCloud.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class LoverConfiguration : IEntityTypeConfiguration<Lover>
    {
        public void Configure(EntityTypeBuilder<Lover> builder)
        {
            builder.HasKey(x => x.Guid);
            builder.Property(x => x.Guid).HasColumnType("varchar(36)");
            builder.ToTable(nameof(Lover));
        }
    }

    internal class LoverLogConfiguration : IEntityTypeConfiguration<LoverLog>
    {
        public void Configure(EntityTypeBuilder<LoverLog> builder)
        {
            builder.HasKey(x => x.Guid);
            builder.Property(x => x.Guid).HasColumnType("varchar(36)");
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
            builder.HasKey(x => x.Guid);
            builder.Property(x => x.Guid).HasColumnType("varchar(36)");
            builder.ToTable(nameof(LoverAnniversary));
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(512);
            builder.Property(x => x.Date).HasColumnType("date");
            builder.HasOne(o => o.Lover)
                .WithMany(o => o.LoverAnniversaries)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
