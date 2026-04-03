using Microsoft.EntityFrameworkCore;
using VenueTracker.Models;

namespace VenueTracker.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Walk> Walks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Venue>(entity =>
            {
                entity.HasKey(v => v.VenueId);
                entity.Property(v => v.VenueName).IsRequired().HasMaxLength(200);
                entity.Property(v => v.City).IsRequired().HasMaxLength(100);
                entity.Property(v => v.State).IsRequired().HasMaxLength(2);
            });

            modelBuilder.Entity<Show>(entity =>
            {
                entity.HasKey(s => s.ShowId);
                entity.Property(s => s.ShowDate).IsRequired();
                entity.HasOne(s => s.Venue)
                      .WithMany(v => v.Shows)
                      .HasForeignKey(s => s.VenueId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Walk>(entity =>
            {
                entity.HasKey(w => w.WalkId);
                entity.Property(w => w.WalkAmount).IsRequired();
                entity.Property(w => w.MerchAmount);
                entity.Property(w => w.Notes);
                entity.HasOne(w => w.Show)
                      .WithMany()
                      .HasForeignKey(w => w.ShowId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}