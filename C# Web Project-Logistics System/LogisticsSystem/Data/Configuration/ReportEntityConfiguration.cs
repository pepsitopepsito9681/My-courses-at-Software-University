using LogisticsSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsSystem.Data.Configuration
{
    public class ReportEntityConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder
                .HasOne(x => x.Load)
                .WithMany(x => x.Reports)
                .HasForeignKey(x => x.LoadId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Reports)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
