using LogisticsSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsSystem.Data.Configuration
{
    public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .HasOne(x => x.Review)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);


            builder
                 .HasOne(x => x.User)
                 .WithMany(x => x.Comments)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
