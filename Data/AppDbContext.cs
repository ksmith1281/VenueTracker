using Microsoft.EntityFrameworkCore;
using VenueTracker.Models;

namespace VenueTracker.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<tVenue> tVenues { get; set; }
        public DbSet<tShow> tShows { get; set; }
        public DbSet<tBuyer> tBuyers { get; set; }
        public DbSet<tSubcontractor> tSubcontractors { get; set; }
        public DbSet<tStatus> tStatuses { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tVenue>(entity =>
            {
                entity.HasKey(v => v.VenueId);
                entity.Property(v => v.VenueName).IsRequired().HasMaxLength(200);
                entity.Property(v => v.City).IsRequired().HasMaxLength(100);
                entity.Property(v => v.State).IsRequired().HasMaxLength(2);
            });

            modelBuilder.Entity<tShow>(entity =>
            {
                entity.HasKey(s => s.ShowId);
                entity.Property(s => s.ShowDate).IsRequired();
                entity.Property(s => s.WalkAmount).HasDefaultValue(0m);
                entity.Property(s => s.MerchAmount);
                entity.Property(s => s.Notes).HasMaxLength(1000);
                entity.HasOne(s => s.tVenue)
                      .WithMany(v => v.tShows)
                      .HasForeignKey(s => s.VenueId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(s => s.tStatus)
                      .WithMany(st => st.tShows)
                      .HasForeignKey(s => s.StatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<tStatus>(entity =>
            {
                entity.HasKey(st => st.StatusId);
                entity.Property(st => st.StatusName).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<tBuyer>(entity =>
            {
                entity.HasKey(b => b.BuyerId);
                entity.Property(b => b.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(b => b.LastName).IsRequired().HasMaxLength(100);
                entity.Property(b => b.Email).HasMaxLength(200);
                entity.Property(b => b.Phone).HasMaxLength(20);
                entity.Property(b => b.Cell).HasMaxLength(20);
            });

            modelBuilder.Entity<tSubcontractor>(entity =>
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