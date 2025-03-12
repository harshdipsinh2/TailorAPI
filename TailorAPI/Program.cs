using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TailorAPI.Models;
using TailorAPI.Repositories;
using TailorAPI.Services;
using TailorAPI.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configure Services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Configure Database
builder.Services.AddDbContext<TailorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// ✅ Configure CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Update with your frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ✅ Register Services & Repositories
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMeasurementService, MeasurementService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IFabricService, FabricService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<MeasurementRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<MeasurementService>();
builder.Services.AddScoped<FabricRepository>();
//builder.Services.AddScoped<JwtService>();

//builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("TailorOnly", policy => policy.RequireRole("Tailor"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
});

// ✅ Build Application
var app = builder.Build();

// ✅ Seed Roles & Admin on Startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TailorDbContext>();
    EnsureAdminExists(dbContext);
}

// ✅ Middleware Configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


static void EnsureAdminExists(TailorDbContext context)
{
    context.Database.Migrate(); // Ensure DB is up-to-date

    // ✅ Check if "Admin" Role Exists
    var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
    if (adminRole == null)
    {
        adminRole = new Role { RoleName = "Admin" };
        context.Roles.Add(adminRole);
        context.SaveChanges();
    }

    // ✅ Check if Admin User Exists
    var adminUser = context.Users.FirstOrDefault(u => u.RoleID == adminRole.RoleID);
    if (adminUser == null)
    {
        var newAdmin = new User
        {
           
            Name = "Admin",
            Email = "admin@shop.com",
            MobileNo = "989898989",
            PasswordHash = HashPassword("Admin@123"), // Secure password hashing
            RoleID = adminRole.RoleID,
            Address = "Shop"
        };

        context.Users.Add(newAdmin);
        context.SaveChanges();
        Console.WriteLine("✅ Admin user created.");
    }
    else
    {
        Console.WriteLine("✅ Admin user already exists.");
    }
}

/// ✅ Securely Hash Passwords
static string HashPassword(string password)
{
    using var sha256 = System.Security.Cryptography.SHA256.Create();
    var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    return Convert.ToBase64String(hashedBytes);
}
