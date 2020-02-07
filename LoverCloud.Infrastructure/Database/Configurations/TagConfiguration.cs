namespace LoverCloud.Infrastructure.Database.Configurations
{
    using LoverCloud.Core.Extensions;
    using LoverCloud.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable(nameof(Tag));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(LoverCloudApiConstraint.IdLength);

            builder.Property(x => x.Name)
                .HasMaxLength(Tag.NameMaxLength);
            
            builder.HasOne(o => o.LoverAlbum)
                .WithMany(o => o.Tags)
                .HasForeignKey(o => o.LoverAlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.LoverPhoto)
                .WithMany(o => o.Tags)
                .HasForeignKey(o => o.LoverPhotoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
