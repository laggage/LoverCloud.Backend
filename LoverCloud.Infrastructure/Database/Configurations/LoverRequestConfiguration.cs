namespace LoverCloud.Infrastructure.Database.Configurations
{
    using LoverCloud.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class LoverRequestConfiguration : IEntityTypeConfiguration<LoverRequest>
    {
        public void Configure(EntityTypeBuilder<LoverRequest> builder)
        {
            builder.HasKey(x => x.Guid);
            builder.Property(x => x.Guid).HasColumnType("varchar(36)");
            builder.ToTable(nameof(LoverRequest));
            builder.HasOne(x => x.Requester)
                .WithOne()
                .HasForeignKey<LoverRequest>(x => x.RequesterGuid)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Receiver)
                .WithMany(x => x.ReceivedLoverRequests)
                .HasForeignKey(x => x.ReceiverGuid)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Lover)
                .WithOne(o => o.LoverRequest)
                .HasForeignKey<LoverRequest>(x => x.LoverGuid)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
