using Microsoft.EntityFrameworkCore;
using VenueTracker.Models;

namespace VenueTracker.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Venue> Venues { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}