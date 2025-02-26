using Microsoft.EntityFrameworkCore;

public class TailorDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Measurement> Measurements { get; set; }

    // ✅ Constructor to Accept DbContextOptions
    public TailorDbContext(DbContextOptions<TailorDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Measurement>()
            .HasOne(m => m.Customer)
            .WithOne(c => c.Measurement)
            .HasForeignKey<Measurement>(m => m.CustomerID)
            .OnDelete(DeleteBehavior.NoAction); // ❌ Prevent Cascade Delete
    }


}
