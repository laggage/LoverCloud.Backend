namespace LoverCloud.Infrastructure.Database.Configurations
{
    using LoverCloud.Core.Extensions;
    using LoverCloud.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class LoverRequestConfiguration : IEntityTypeConfiguration<LoverRequest>
    {
        public void Configure(EntityTypeBuilder<LoverRequest> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasMaxLength(LoverCloudApiConstraint.IdLength);

            builder.ToTable(nameof(LoverRequest));
            builder.HasOne(x => x.Requester)
                .WithMany(x => x.LoverRequests)
                .HasForeignKey(x => x.RequesterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Receiver)
                .WithMany(x => x.ReceivedLoverRequests)
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Lover)
                .WithOne(o => o.LoverRequest)
                .HasForeignKey<LoverRequest>(x => x.LoverId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
