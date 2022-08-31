using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;
using HotelListing.Data;
using HotelListing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HotelListing.Services;

public class AuthManager : IAuthManager
{
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _configuration;
    private ApiUser _user;
    
    public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
    {
        this._userManager = userManager;
        this._configuration = configuration;
    }
    
    public async Task<bool> ValidateUser(UserLoginDTO userDTO)
    {
        _user = await _userManager.FindByNameAsync(userDTO.Email);

        return (_user != null && await _userManager.CheckPasswordAsync(_user, userDTO.Password));
    }
    
    public async Task<string> CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
    
    private SigningCredentials GetSigningCredentials()
    {
        var key = Environment.GetEnvironmentVariable("JWT_KEY");
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, _user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(_user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        
        // returning jwt token
        return new JwtSecurityToken(
            issuer: jwtSettings.GetSection("Issuer").Value,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Int32.Parse(jwtSettings.GetSection("ExpiresIn").Value)),
            signingCredentials: signingCredentials
        );
    }
}