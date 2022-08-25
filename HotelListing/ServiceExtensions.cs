using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using HotelListing.Data;

namespace HotelListing;

public static class ServiceExtensions
{
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<ApiUser>(q => q.User.RequireUniqueEmail = true);
        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
        
        builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
    }
}