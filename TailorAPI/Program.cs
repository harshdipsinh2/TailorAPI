using Microsoft.EntityFrameworkCore;
using TailorAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<TailorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<MeasurementService>(); // ✅ Register MeasurementService

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });
 


// Register Repositories (Without Interfaces)
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<MeasurementRepository>();

// Register Services (Without Interfaces)
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<MeasurementService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
