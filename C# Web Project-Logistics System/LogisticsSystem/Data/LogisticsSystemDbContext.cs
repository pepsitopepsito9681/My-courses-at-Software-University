using LogisticsSystem.Data.Configuration;
using LogisticsSystem.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.Data
{
    public class LogisticsSystemDbContext : IdentityDbContext<User>
    {
        public LogisticsSystemDbContext(DbContextOptions<LogisticsSystemDbContext> options)
            : base(options)
        {
        }
        public DbSet<Response> Responses { get; set; }

        public DbSet<Load> Loads { get; set; }

        public DbSet<Kind> Kinds { get; set; }

        public DbSet<SubKind> SubKinds { get; set; }

        public DbSet<Trader> Traders { get; set; }

        public DbSet<LoadImage> LoadsImages { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Favourite> Favourites { get; set; }

        public DbSet<DeliveryCartItem> DeliveryCarts { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {


            builder.ApplyConfiguration(new LoadEntityConfiguration());

            builder.ApplyConfiguration(new ReviewEntityConfiguration());

            builder.ApplyConfiguration(new QuestionEntityConfiguration());

            builder.ApplyConfiguration(new ResponseEntityConfiguration());

            builder.ApplyConfiguration(new CommentEntityConfiguration());

            builder.ApplyConfiguration(new FavouriteEntityConfiguration());

            builder.ApplyConfiguration(new DeliveryCartItemEntityConfiguration());

            builder.ApplyConfiguration(new ReportEntityConfiguration());

            builder
              .Entity<SubKind>()
              .HasOne(x => x.Kind)
              .WithMany(x => x.SubKinds)
              .HasForeignKey(x => x.KindId)
              .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Trader>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Trader>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LoadImage>()
                .HasOne(x => x.Load)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.LoadId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.Entity<Order>()
                .HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(builder);
        }
    }
}
