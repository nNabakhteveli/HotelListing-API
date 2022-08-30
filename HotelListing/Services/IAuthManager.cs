using HotelListing.Models;

namespace HotelListing.Services;

public interface IAuthManager
{
    Task<bool> ValidateUser(UserLoginDTO userDTO);
    Task<string> CreateToken();
}