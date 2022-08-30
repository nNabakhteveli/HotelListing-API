using HotelListing.Data;
using HotelListing.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Services;

public class AuthManager : IAuthManager
{
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
    {
        this._userManager = userManager;
        this._configuration = configuration;
    }
    
    public async Task<bool> ValidateUser(UserLoginDTO userDTO)
    {
        var user = await _userManager.FindByNameAsync(userDTO.Email);

        return (user != null && await _userManager.CheckPasswordAsync(user, userDTO.Password));
    }

    public Task<string> CreateToken()
    {
        throw new NotImplementedException();
    }
}