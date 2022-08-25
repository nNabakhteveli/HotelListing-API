using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        
        builder.Entity<Country>().HasData(
            new Country
            {
                Id = 1,
                Name = "Georgia",
                ShortName = "GEO"
            },
            new Country
            {
                Id = 2,
                Name = "Germany",
                ShortName = "DEU"
            },
            new Country
            {
                Id = 3,
                Name = "Japan",
                ShortName = "JPN"
            }
        );

        builder.Entity<Hotel>().HasData(
            new Hotel
            {
                Id = 1,
                Name = "Hotel 1",
                Address = "Hotel 1 Address",
                Rating = 4.5,
                CountryId = 1,
            },
            new Hotel
            {
                Id = 2,
                Name = "Hotel 2",
                Address = "Hotel 2 Address",
                Rating = 4.9,
                CountryId = 2,
            },
            new Hotel
            {
                Id = 3,
                Name = "Hotel 3",
                Address = "Hotel 3 Address",
                Rating = 4.4,
                CountryId = 1,
            },
            new Hotel
            {
                Id = 4,
                Name = "Hotel 4",
                Address = "Hotel 4 Address",
                Rating = 4.7,
                CountryId = 3,
            }
        );
    }
}