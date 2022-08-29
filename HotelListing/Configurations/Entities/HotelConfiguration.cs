using HotelListing.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Configurations.Entities;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasData(
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