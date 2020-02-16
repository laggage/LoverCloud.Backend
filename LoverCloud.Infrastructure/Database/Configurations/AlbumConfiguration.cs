namespace LoverCloud.Infrastructure.Database.Configurations
{
    using LoverCloud.Core.Extensions;
    using LoverCloud.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class LoverAlbumConfiguration : IEntityTypeConfiguration<LoverAlbum>
    {
        public void Configure(EntityTypeBuilder<LoverAlbum> builder)
        {
            builder.ToTable(nameof(LoverAlbum));

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasMaxLength(LoverCloudApiConstraint.IdLength);

            builder.Property(x => x.Name)
                .HasMaxLength(LoverAlbum.NameMaxLength);
            builder.Property(x => x.Description)
                .HasMaxLength(LoverAlbum.DescriptionMaxLength);

            builder.HasOne(o => o.Lover)
                .WithMany(o => o.LoverAlbums)
                .HasForeignKey(o => o.LoverId);
            builder.HasOne(album => album.Creater)
                .WithMany(o => o.LoverAlbums)
                .HasForeignKey(album => album.CreaterId);
        }
    }

    internal class LoverPhotoConfiguration : IEntityTypeConfiguration<LoverPhoto>
    {
        public void Configure(EntityTypeBuilder<LoverPhoto> builder)
        {
            builder.ToTable(nameof(LoverPhoto));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasMaxLength(LoverCloudApiConstraint.IdLength);
            builder.Property(x => x.Name)
                .HasMaxLength(LoverPhoto.NameMaxLength);
            builder.Property(x => x.Description)
                .HasMaxLength(LoverPhoto.DescriptionMaxLength);
            builder.Property(x => x.PhotoUrl)
                .HasMaxLength(LoverPhoto.PhotoUrlMaxLength);
            builder.Property(x => x.PhysicalPath)
                .HasMaxLength(LoverPhoto.PhotoPhysicalPathMaxLength);

            builder.HasOne(o => o.Lover)
                .WithMany(o => o.LoverPhotos);
            builder.HasOne(o => o.Album)
                .WithMany(o => o.Photos)
                .HasForeignKey(o => o.AlbumId);
            builder.HasOne(x => x.LoverLog)
                .WithMany(x => x.LoverPhotos);
            builder.HasOne(photo => photo.Uploader)
                .WithMany(user => user.LoverPhotos)
                .HasForeignKey(photo => photo.UploaderId);
        }
    }
}
