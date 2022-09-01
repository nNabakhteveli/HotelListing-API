using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Serilog;
using AutoMapper;
using Serilog.Events;

using HotelListing;
using HotelListing.Configurations;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Repository;
using HotelListing.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"))
);

DotNetEnv.Env.Load();

builder.Host.UseSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthManager, AuthManager>();

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAllPolicy", builder => 
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});
builder.Services.AddAutoMapper(typeof(MapperInitializer));
builder.Services.AddControllers().AddNewtonsoftJson(options => 
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        path: "/Users/nika/Desktop/dotnet/HotelListing/HotelListing/Logs/log-.txt",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:1j}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information
    ).CreateLogger();

try
{
    Log.Information("Application is starting");
}
catch (Exception e)
{
    Log.Fatal("Application failed to start");
    Console.WriteLine(e);
    throw;
}
finally
{
    Log.CloseAndFlush();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { }

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAllPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();