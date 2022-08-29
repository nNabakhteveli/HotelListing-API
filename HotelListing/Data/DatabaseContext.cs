using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using HotelListing.Configurations.Entities;

namespace HotelListing.Data;

// Bridge between application and database
public class DatabaseContext : IdentityDbContext<ApiUser>
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Country> Countries { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfiguration(new CountryConfiguration());
        builder.ApplyConfiguration(new HotelConfiguration());
    }
}