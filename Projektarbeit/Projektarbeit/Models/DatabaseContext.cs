using Microsoft.EntityFrameworkCore;

namespace Projektarbeit.Models;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }

    public DatabaseContext()
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Booking>()
            .Property(x => x.Time)
            .HasConversion(
                x => x.ToString(),
                x => (Time) Enum.Parse(typeof(Time), x));
        
        modelBuilder
            .Entity<Booking>()
            .Property(x => x.Status)
            .HasConversion(
                x => x.ToString(),
                x => (Status) Enum.Parse(typeof(Status), x));
    }

    public DbSet<Booking> Bookings { get; set; }
    public DbSet<User> Users { get; set; }
}