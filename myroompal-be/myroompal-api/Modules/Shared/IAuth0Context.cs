using System.Security.Claims;
using myroompal_api.Entities.Entities;

namespace myroompal_api.Modules.Shared;

public interface IAuth0Context
{
    string GetAuth0Id(ClaimsPrincipal user);
    bool IsAdmin(ClaimsPrincipal user);
}