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
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


builder.Services.AddEndpointsApiExplorer();

// ✅ Configure Swagger with JWT Authentication
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tailor API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ✅ Configure Database
builder.Services.AddDbContext<TailorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ✅ Configure CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:3000")
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
//builder.Services.AddScoped<IFabricService, FabricService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IFabricCombinedService, FabricCombinedService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOtpVerificationService, OtpVerificationService>();

builder.Services.AddScoped<OtpVerificationRepository>();
builder.Services.AddScoped<ManagerRepository>();
builder.Services.AddScoped<FabricTypeCombinedRepository>();
builder.Services.AddScoped<AdminRepository>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<MeasurementRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<MeasurementService>();
//builder.Services.AddScoped<FabricRepository>();

// ✅ JWT Authentication Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])
            )
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
    context.Database.Migrate();

    var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
    if (adminRole == null)
    {
        adminRole = new Role { RoleName = "Admin" };
        context.Roles.Add(adminRole);
        context.SaveChanges();
    }
     string HashPassword(string password)
{
    string salt = BCrypt.Net.BCrypt.GenerateSalt();
    return BCrypt.Net.BCrypt.HashPassword(password, salt);
}
var adminUser = context.Users.FirstOrDefault(u => u.RoleID == adminRole.RoleID);
    if (adminUser == null)
    {
        var newAdmin = new User
        {
            Name = "Admin",
            Email = "adminshop@ptct.net",
            MobileNo = "989898989",
            PasswordHash = HashPassword("Admin@1234"),
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
