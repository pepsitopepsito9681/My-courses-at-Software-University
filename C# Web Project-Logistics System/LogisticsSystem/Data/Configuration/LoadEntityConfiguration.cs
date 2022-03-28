using LogisticsSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsSystem.Data.Configuration
{
    public class LoadEntityConfiguration : IEntityTypeConfiguration<Load>
    {
        public void Configure(EntityTypeBuilder<Load> builder)
        {
            builder
                 .HasOne(x => x.SubKind)
                 .WithMany(x => x.Loads)
                 .HasForeignKey(x => x.SubKindId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder
                  .HasOne(x => x.Kind)
                  .WithMany(x => x.Loads)
                  .HasForeignKey(x => x.KindId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder
                  .HasOne(x => x.Trader)
                  .WithMany(x => x.Loads)
                  .HasForeignKey(x => x.TraderId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
