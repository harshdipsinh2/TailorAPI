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
    //public DbSet<Fabric> Fabrics { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<FabricType> FabricTypes { get; set; }
    public DbSet<FabricStock> FabricStocks { get; set; }
    public DbSet<OtpVerification> OtpVerifications { get; set; }

    public DbSet<Branch> Branches { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<TwilioSms> TwilioSms { get; set; }
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

            modelBuilder.Entity<FabricType>()
    .Property(f => f.PricePerMeter)
    .HasColumnType("decimal(18,2)");

                    

            // ✅ Customer Foreign Keys (Fix multiple cascade paths)
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Shop)
                .WithMany(s => s.Customers)
                .HasForeignKey(c => c.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Branch)
                .WithMany(b => b.Customers)
                .HasForeignKey(c => c.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Soft Delete Filters
            modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);




            /////new added if not work 


            modelBuilder.Entity<FabricStock>()
    .Property(f => f.StockIn)
    .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<FabricStock>()
                .Property(f => f.StockUse)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<FabricType>()
                .Property(f => f.AvailableStock)
                .HasColumnType("decimal(18,2)");


            ///// remove upeer one 


            {
                modelBuilder.Entity<Product>()
                    .Property(p => p.ProductType)
                    .HasConversion<string>(); // 👈 This stores the enum as string

                // Optional: Set string length (VARCHAR size)
                modelBuilder.Entity<Product>()
                    .Property(p => p.ProductType)
                    .HasMaxLength(20);
            }

            {
                modelBuilder.Entity<TwilioSms>()
                    .Property(e => e.SmsType)
                    .HasConversion<string>();

                base.OnModelCreating(modelBuilder);
            }

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

            entity.HasOne(o => o.fabricType)
                .WithMany(f => f.Orders)
                .HasForeignKey(o => o.FabricTypeID)
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
        modelBuilder.Entity<FabricType>().HasQueryFilter(f => !f.IsDeleted);
        modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);

        // ✅ Seed Roles
        modelBuilder.Entity<Role>().HasData(

            new Role { RoleID = 1, RoleName = "SuperAdmin" },
            new Role { RoleID = 2, RoleName = "Admin" },
            new Role { RoleID = 3, RoleName = "Manager" },
            new Role { RoleID = 4, RoleName = "Tailor" }


        );
    }
}




