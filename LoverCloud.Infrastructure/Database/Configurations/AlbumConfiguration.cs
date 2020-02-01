namespace LoverCloud.Infrastructure.Database.Configurations
{
    using LoverCloud.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class LoverAlbumConfiguration : IEntityTypeConfiguration<LoverAlbum>
    {
        public void Configure(EntityTypeBuilder<LoverAlbum> builder)
        {
            builder.HasKey(x => x.Guid);
            builder.Property(x => x.Guid).HasColumnType("varchar(36)");
            builder.ToTable(nameof(LoverAlbum));
            builder.HasOne(o => o.Lover)
                .WithMany(o => o.LoverAlbums)
                .HasForeignKey(o => o.LoverGuid);
        }
    }

    internal class LoverPhotoConfiguration : IEntityTypeConfiguration<LoverPhoto>
    {
        public void Configure(EntityTypeBuilder<LoverPhoto> builder)
        {
            builder.HasKey(x => x.Guid);
            builder.Property(x => x.Guid).HasColumnType("varchar(36)");
            builder.ToTable(nameof(LoverPhoto));
            builder.HasOne(o => o.Lover)
                .WithMany(o => o.LoverPhotos);
            builder.HasOne(o => o.Album)
                .WithMany(o => o.Photos)
                .HasForeignKey(o => o.AlbumGuid);
            builder.HasOne(x => x.LoverLog)
                .WithMany(x => x.LoverPhotos);
        }
    }
}
