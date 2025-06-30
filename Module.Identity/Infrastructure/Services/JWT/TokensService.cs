using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Module.Identity.Domain.Models;
using Module.Identity.Infrastructure.JWT;
using Module.Identity.Persistence;
using Module.Identity.Persistence.JWT;

namespace Module.Identity.Infrastructure.Services.JWT;

public class TokensService
        (JwtSettings jwtSettings, UserManager<ApplicationUser> userManager, IdentityModuleDbContext context) :
        ITokensService
{
    #region Public Methods
    public async Task<RefreshToken?> SaveRefreshTokenAsync(
        long userId,
        string token,
        DateTimeOffset creationDate,
        DateTimeOffset expiryDate)
    {
        try
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = token,
                CreationDate = creationDate,
                ExpiryDate = expiryDate,
            };
            
            await context.RefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();
            
            return refreshToken;
        }
        catch
        {
            return null;
        }
    }

    public async Task<(long UserId, string AccessToken, DateTimeOffset AccessTokenExpiry, string RefreshToken, DateTimeOffset RefreshTokenExpiry)?> GenerateTokensAsync(ApplicationUser user)
    {
        var (accessToken, accessExpiry) = await GenerateAccessToken(user);
        var (refreshToken, refreshCreationDate, refreshExpiry) = GenerateRefreshToken();
        
        var savedToken = await SaveRefreshTokenAsync(user.Id, refreshToken, refreshCreationDate, refreshExpiry);
        if (savedToken == null)
        {
            return null;
        }

        return (user.Id, accessToken, accessExpiry, refreshToken, refreshExpiry);
    }
    
    public async Task<bool> RemoveRefreshTokenAsync(string refreshToken)
    {
        var token = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token.Equals(refreshToken));
        if (token is null) return false;

        context.RefreshTokens.Remove(token);
        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<int> RemoveExpiredTokensAsync()
    {
        try
        {
            var rowsAffected = await context.RefreshTokens
                .Where(t => DateTime.UtcNow >= t.ExpiryDate || t.RevokedOn.HasValue)
                .ExecuteDeleteAsync();

            if (rowsAffected > 0)
            {
                Console.WriteLine($"{rowsAffected} expired or revoked tokens removed.");
            }
            
            return rowsAffected;
        }
        catch
        {
            return 0;
        }
    }
    
    public async Task<(long UserId, string AccessToken, DateTimeOffset AccessTokenExpiry, string RefreshToken, DateTimeOffset RefreshTokenExpiry)?> RefreshAsync(string accessToken, string refreshToken)
    {
        if (!await ValidateAccessTokenAsync(accessToken))
            return null;

        if (!await ValidateRefreshTokenAsync(refreshToken))
            return null;

        var rt = await context.RefreshTokens
            .Include(rt => rt.User)
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.Token.Equals(refreshToken));
            
        var user = rt?.User;
        if (user is null)
        {
            return null;
        }
        
        // Revoke the token
        rt.RevokedOn = DateTime.UtcNow;
        context.RefreshTokens.Update(rt);
        
        try
        {
            await context.SaveChangesAsync();
        }
        catch
        {
            return null;
        }

        return await GenerateTokensAsync(user);
    }
    
    public static TokenValidationParameters GetTokenValidationParameters(JwtSettings settings, bool validateLifetime = false) =>
    new TokenValidationParameters()
    {
        ValidateIssuer = settings.ValidateIssuer,
        ValidIssuer = settings.Issuer,

        ValidateAudience = settings.ValidateAudience,
        ValidAudience = settings.Audiences.FirstOrDefault(),

        ClockSkew = TimeSpan.FromMinutes(5),

        ValidateLifetime = validateLifetime,
        IssuerSigningKey = settings.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true
    };
    #endregion
    
    #region Private Methods
    private async Task<(string Token, DateTime ExpiryDate)> GenerateAccessToken(ApplicationUser user)
    {
        var expires = DateTime.UtcNow.AddMinutes(jwtSettings.TokenExpMinutes);
        var claims = await GetClaimsAsync(user);
        var jwt = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audiences[0],
            claims: claims,
            expires: expires,
            signingCredentials: new SigningCredentials(jwtSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return (encodedJwt, expires);
    }
    
    private (string Token, DateTimeOffset CreationDate, DateTimeOffset ExpiryDate) GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(randomNumber);
            var now = DateTimeOffset.UtcNow;
            return (
                Convert.ToBase64String(randomNumber),
                now,
                now.AddMinutes(jwtSettings.RefreshTokenExpMinutes)
            );
        }
    }
    
    private async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
    {
        // Simplified claims without roles or permissions
        List<Claim> claims = [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        ];

        return claims;
    }

    private async Task<bool> ValidateAccessTokenAsync(string accessToken)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrincipal = tokenHandler.ValidateToken(accessToken, GetTokenValidationParameters(jwtSettings), out var _);
            var claims = claimsPrincipal.Claims.ToList();

            var hasId = long.TryParse(claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value, out long userIdClaim);
            if (!hasId) return false;

            var userEmailClaim = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email));
            if (userEmailClaim is null) return false;

            var userExist = await userManager.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == userIdClaim && u.Email.Equals(userEmailClaim.Value));
            return userExist;
        }
        catch
        {
            return false;
        }
    }
    
    private async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
    {
        var token = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token.Equals(refreshToken));
        return token is not null && token.IsActive;
    }
    #endregion
}