using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi.Extensions;

public static class ClaimExtensions
{
    public static string GetUserName(this ClaimsPrincipal user)
    {
        return user.Claims.SingleOrDefault(x => x.Type.Equals("username"))?.Value;
    }
}