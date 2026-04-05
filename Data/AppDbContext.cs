using Microsoft.EntityFrameworkCore;
using VenueTracker.Models;

namespace VenueTracker.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Subcontractor> Subcontractors { get; set; }

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

            modelBuilder.Entity<Buyer>(entity =>
            {
                entity.HasKey(b => b.BuyerId);
                entity.Property(b => b.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(b => b.LastName).IsRequired().HasMaxLength(100);
                entity.Property(b => b.Email).HasMaxLength(200);
                entity.Property(b => b.Phone).HasMaxLength(20);
                entity.Property(b => b.Cell).HasMaxLength(20);
            });

            modelBuilder.Entity<Subcontractor>(entity =>
            {
                entity.HasKey(s => s.SubcontractorId);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
                entity.Property(s => s.Role).HasMaxLength(100);
                entity.Property(s => s.Email).HasMaxLength(200);
                entity.Property(s => s.Phone).HasMaxLength(20);
                entity.Property(s => s.Notes).HasMaxLength(500);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}