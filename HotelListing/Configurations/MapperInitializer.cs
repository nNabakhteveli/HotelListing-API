using AutoMapper;

using HotelListing.Data;
using HotelListing.Models;
using HotelListing.Models.Hotel;
using HotelListing.Models.User;

namespace HotelListing.Configurations;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {
        CreateMap<Country, CountryDTO>().ReverseMap();
        CreateMap<Country, CreateCountryDTO>().ReverseMap();
        CreateMap<Hotel, HotelDTO>().ReverseMap();
        CreateMap<Hotel, CreateHotelDTO>().ReverseMap();
        CreateMap<ApiUser, UserRegistrationDTO>().ReverseMap();
    }
}