using LogisticsSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsSystem.Data.Configuration
{
    public class DeliveryCartItemEntityConfiguration : IEntityTypeConfiguration<DeliveryCartItem>
    {
        public void Configure(EntityTypeBuilder<DeliveryCartItem> builder)
        {
            builder
                  .HasOne(x => x.Load)
                  .WithMany(x => x.DeliveryCartItems)
                  .HasForeignKey(x => x.LoadId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.DeliveryCartItems)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Order)
                .WithMany(x => x.DeliveryCart)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
