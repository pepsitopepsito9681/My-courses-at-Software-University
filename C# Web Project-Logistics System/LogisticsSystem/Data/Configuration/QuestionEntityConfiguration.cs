using LogisticsSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsSystem.Data.Configuration
{
    public class QuestionEntityConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder
               .HasOne(x => x.Load)
               .WithMany(x => x.Questions)
               .HasForeignKey(x => x.LoadId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
                   .HasOne(x => x.User)
                   .WithMany(x => x.Questions)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
