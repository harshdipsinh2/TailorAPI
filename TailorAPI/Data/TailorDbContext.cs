using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using TailorAPI.Migrations;
using TailorAPI.Models;

public class TailorDbContext : IdentityDbContext<AppUser>
{
    public TailorDbContext(DbContextOptions<TailorDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Measurement> Measurements { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }

    public DbSet<AppUser> AppUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ✅ Ensure Identity Tables Exist
        modelBuilder.Entity<IdentityRole>().ToTable("AppUser");


        // ✅ Customer Configuration
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerID);
            entity.Property(e => e.FullName).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Address).IsRequired();
            entity.Property(e => e.PhoneNumber).IsRequired();
            entity.Property(e => e.IsDeleted).HasColumnType("bit");
        });

        // ✅ Employee Configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeID);
            entity.Property(e => e.FullName).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Address).IsRequired();
            entity.Property(e => e.PhoneNumber).IsRequired();
            entity.Property(e => e.Role).IsRequired();
            entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Attendance).HasColumnType("int");
            entity.Property(e => e.Status).HasColumnType("int");
            entity.Property(e => e.IsDeleted).HasColumnType("bit");

            // Foreign Keys
            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerID)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Measurement)
                .WithMany()
                .HasForeignKey(e => e.MeasurementID)
                .OnDelete(DeleteBehavior.NoAction);
        });

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

            // Foreign Key
            entity.HasOne(e => e.Customer)
                .WithOne(c => c.Measurement)
                .HasForeignKey<Measurement>(e => e.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)"); // ✅ Fix precision

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)"); // ✅ Fix precision
            {
                modelBuilder.Entity<Product>()
                    .Property(p => p.ProductID)
                    .ValueGeneratedNever(); // Ensures ProductID is not auto-generated
            }
        }
    }
}
