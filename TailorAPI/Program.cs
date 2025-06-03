using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using TailorAPI.Models;
using TailorAPI.Repositories;
using TailorAPI.Services;
using TailorAPI.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Controllers and JSON options (including enums as strings)
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// ✅ Swagger + Enums inline + JWT Auth config
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TailorAPI", Version = "v1" });
    options.UseInlineDefinitionsForEnums(); // ✅ Shows enums directly inside schema

    // ✅ JWT Auth in Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// ✅ Configure DB
builder.Services.AddDbContext<TailorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ✅ CORS Policy
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// ✅ Register Services and Repositories
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMeasurementService, MeasurementService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IFabricCombinedService, FabricCombinedService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOtpVerificationService, OtpVerificationService>();
builder.Services.AddScoped<ITwilioService, TwilioService>();

builder.Services.AddScoped<TwilioRepository>();
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

// ✅ JWT Configuration
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
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// ✅ Authorization Roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("TailorOnly", policy => policy.RequireRole("Tailor"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
});

// ✅ Build App
var app = builder.Build();

// ✅ Seed Roles and Admin User
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TailorDbContext>();
    EnsureAdminExists(dbContext);
}

// ✅ Middleware Pipeline
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

// ✅ Admin Seeding Method
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

