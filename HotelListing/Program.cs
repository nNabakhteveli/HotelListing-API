using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAllPolicy", builder => 
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});
builder.Host.UseSerilog();

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

app.UseAuthorization();

app.MapControllers();

app.Run();