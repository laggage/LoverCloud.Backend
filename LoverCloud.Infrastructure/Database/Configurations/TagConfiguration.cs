namespace LoverCloud.Infrastructure.Database.Configurations
{
    using LoverCloud.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("varchar(36)");
            builder.ToTable(nameof(Tag));
            builder.HasOne(o => o.LoverAlbum)
                .WithMany(o => o.Tags)
                .HasForeignKey(o => o.LoverAlbumGuid)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.LoverPhoto)
                .WithMany(o => o.Tags)
                .HasForeignKey(o => o.LoverPhotoGuid)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.Name)
                .HasMaxLength(256);
        }
    }
}
