using Microsoft.EntityFrameworkCore;

/// <summary>
/// Training Management System Entity Framework DbContext
/// </summary>
public class TMSDbContext : DbContext
{
    public TMSDbContext(DbContextOptions<TMSDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Add entity configurations here
    }
}
