using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Shared.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public long? Id => GetUserId();
    public string Email => GetUserEmail();
    public string UserName => GetUserClaim(JwtRegisteredClaimNames.Name);
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    private long? GetUserId()
    {
        var userId = GetUserClaim(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return null;

        return long.TryParse(userId, out var id) ? id : null;
    }

    private string GetUserEmail()
    {
        return GetUserClaim(JwtRegisteredClaimNames.Email);
    }

    private string GetUserClaim(string claimType)
    {
        return httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType) ?? string.Empty;
    }
}

public interface ICurrentUserService
{
    long? Id { get; }
    string Email { get; }
    string UserName { get; }
   
    bool IsAuthenticated { get; }
}
