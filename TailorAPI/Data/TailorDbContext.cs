using Microsoft.EntityFrameworkCore;
using TailorAPI.Models;

public class TailorDbContext : DbContext
{
    public TailorDbContext(DbContextOptions<TailorDbContext> options)
        : base(options)
    {
    }

    // Define your tables
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Measurement> Measurements { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Fabric> Fabrics { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ✅ Measurement Configuration
        modelBuilder.Entity<Measurement>(entity =>
        {
            entity.HasKey(e => e.MeasurementID);

            entity.Property(e => e.Arms).HasColumnType("real");
            entity.Property(e => e.Chest).HasColumnType("real");
            entity.Property(e => e.Hip).HasColumnType("real");
            entity.Property(e => e.Inseam).HasColumnType("real");
            entity.Property(e => e.Neck).HasColumnType("real");
            entity.Property(e => e.Shoulder).HasColumnType("real");
            entity.Property(e => e.Sleeve).HasColumnType("real");
            entity.Property(e => e.SleeveLength).HasColumnType("real");
            entity.Property(e => e.Thigh).HasColumnType("real");
            entity.Property(e => e.TrouserLength).HasColumnType("real");
            entity.Property(e => e.Waist).HasColumnType("real");
            entity.Property(e => e.IsDeleted).HasColumnType("bit");

            // ✅ Correct Foreign Key
            modelBuilder.Entity<Customer>()
         .HasOne(c => c.Measurement)
         .WithOne(m => m.Customer)
         .HasForeignKey<Measurement>(m => m.CustomerId)
         .IsRequired(false); // Make it optional

            modelBuilder.Entity<Fabric>()
    .Property(f => f.PricePerMeter)
    .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Fabric>()
                .Property(f => f.StockQuantity)
                .HasColumnType("decimal(18,2)");


        });

        // ✅ Order Entity Configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.OrderID);

            entity.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
            entity.Property(o => o.FabricLength).HasColumnType("decimal(18,2)");

            entity.HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(o => o.Product)
                       .WithMany(p => p.Orders)
                       .HasForeignKey(o => o.ProductID)
                       .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(o => o.Fabric)
                .WithMany(f => f.Orders)
                .HasForeignKey(o => o.FabricID)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(o => o.Assigned)
                .WithMany()
                .HasForeignKey(o => o.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ✅ Product Configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.ProductID);
            entity.Property(p => p.MakingPrice).HasColumnType("decimal(18,2)");
        });
        // ✅ User Entity Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserID);

            entity.Property(u => u.UserStatus)
                .HasDefaultValue(UserStatus.Available); // Default Status
        });
        // ✅ Soft Delete Filters
        modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Fabric>().HasQueryFilter(f => !f.IsDeleted);
        modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);

        // ✅ Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleID = 1, RoleName = "Admin" },
            new Role { RoleID = 2, RoleName = "Tailor" },
            new Role { RoleID = 3, RoleName = "Manager" }
        );
    }
}