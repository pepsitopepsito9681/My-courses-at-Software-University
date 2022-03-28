using LogisticsSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsSystem.Data.Configuration
{
    public class FavouriteEntityConfiguration : IEntityTypeConfiguration<Favourite>
    {
        public void Configure(EntityTypeBuilder<Favourite> builder)
        {
            builder
                 .HasOne(x => x.Load)
                 .WithMany(x => x.Favourites)
                 .HasForeignKey(x => x.LoadId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Favourites)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
