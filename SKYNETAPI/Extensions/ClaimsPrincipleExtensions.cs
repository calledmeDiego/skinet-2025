using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SKYNETCORE.Entities;
using System.Security.Authentication;
using System.Security.Claims;

namespace SKYNETAPI.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager, ClaimsPrincipal user)
    {
        var userResponse = await userManager.Users.FirstOrDefaultAsync(x => x.Email == user.GetEmail());

        if (userResponse == null) throw new AuthenticationException("User not found");

        return userResponse; 
    }

    public static async Task<AppUser> GetUserByEmailWithAddress(this UserManager<AppUser> userManager, ClaimsPrincipal user)
    {
        var userResponse = await userManager.Users
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Email == user.GetEmail());

        if (userResponse == null) throw new AuthenticationException("User not found");

        return userResponse;
    }

    public static string GetEmail(this ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email) ?? throw new AuthenticationException("Email claim not found");

        return email;
    }
}
