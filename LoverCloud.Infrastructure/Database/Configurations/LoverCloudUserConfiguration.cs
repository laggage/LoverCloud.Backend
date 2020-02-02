namespace LoverCloud.Infrastructure.Database.Configurations
{
    using LoverCloud.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

    internal class LoverCloudUserConfiguration : IEntityTypeConfiguration<LoverCloudUser>
    {
        public void Configure(EntityTypeBuilder<LoverCloudUser> builder)
        {
            builder.ToTable(nameof(LoverCloudUser));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("varchar(36)");
            builder.Property(x => x.Birth)
                .HasColumnType("date");
            builder.Property(x => x.Sex)
                .HasConversion(new EnumToStringConverter<Sex>());
        }
    }
}
