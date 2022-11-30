using Microsoft.EntityFrameworkCore;

namespace Punchclock.Models;

public class PunchclockDbContext : DbContext
{
    public PunchclockDbContext(DbContextOptions<PunchclockDbContext> options)
        : base(options)
    {
    }

    
    public DbSet<Entry> Entries { get; set; }   
}