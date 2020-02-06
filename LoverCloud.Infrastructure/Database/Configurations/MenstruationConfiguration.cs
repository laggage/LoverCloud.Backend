namespace LoverCloud.Infrastructure.Database.Configurations
{
    using LoverCloud.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class MenstruationConfiguration : IEntityTypeConfiguration<MenstruationLog>
    {
        public void Configure(EntityTypeBuilder<MenstruationLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("varchar(36)");
            builder.ToTable(nameof(MenstruationLog));
            builder.HasOne(x => x.LoverCloudUser)
                .WithMany(x => x.MenstruationLogs)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    internal class MenstruationDescriptionConfiguration : IEntityTypeConfiguration<MenstruationDescription>
    {
        public void Configure(EntityTypeBuilder<MenstruationDescription> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("varchar(36)");
            builder.ToTable(nameof(MenstruationDescription));
            builder.HasOne(o => o.MenstruationLog)
                .WithMany(o => o.MenstruationDescriptions)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.Description).HasMaxLength(512);
        }
    }
}
