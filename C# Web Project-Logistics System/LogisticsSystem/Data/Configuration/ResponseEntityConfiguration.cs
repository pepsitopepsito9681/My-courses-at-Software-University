using LogisticsSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsSystem.Data.Configuration
{
    public class ResponseEntityConfiguration : IEntityTypeConfiguration<Response>
    {
        public void Configure(EntityTypeBuilder<Response> builder)
        {
            builder
                   .HasOne(x => x.User)
                   .WithMany(x => x.Responses)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder
                   .HasOne(x => x.Question)
                   .WithMany(x => x.Responses)
                   .HasForeignKey(x => x.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
