using Microsoft.EntityFrameworkCore;

namespace Punchclock.Models;

public class PunchclockDbContext : DbContext
{
    public PunchclockDbContext(DbContextOptions<PunchclockDbContext> options)
        : base(options)
    {
    }

    public PunchclockDbContext()
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=db.db");
        }
    }
    
    public DbSet<Entry> Entries { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
}