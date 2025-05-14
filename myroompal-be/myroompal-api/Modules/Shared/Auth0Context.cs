using System.Security.Claims;
using myroompal_api.Data;
using myroompal_api.Entities.Entities;
using myroompal_api.Entities.Types;

namespace myroompal_api.Modules.Shared;

public class Auth0Context : IAuth0Context
{
    private readonly ApplicationDbContext _context;

    public Auth0Context(ApplicationDbContext context)
    {
        _context = context;
    }
    public string GetAuth0Id(ClaimsPrincipal user)
    {
        // Extract Auth0Id from claims
        var auth0Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(auth0Id))
        {
            throw new UnauthorizedAccessException("No Auth0 ID found in token.");
        }

        return auth0Id;

    }

    public bool IsAdmin(ClaimsPrincipal user)
    {
        var auth0Id = GetAuth0Id(user);
        // Check if user is an admin
        var userEntity = _context.Users.FirstOrDefault(u => u.Auth0Id == auth0Id);
        if (userEntity == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        return userEntity.RoleName == UserRoleType.Moderator;
    }
}